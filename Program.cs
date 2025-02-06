using FinanceControl.FinanceControl.API.Middlewares;
using FinanceControl.FinanceControl.Application.Common;
using FinanceControl.FinanceControl.Application.Security;
using FinanceControl.FinanceControl.Application.Services;
using FinanceControl.FinanceControl.Domain.Interfaces.Repositories;
using FinanceControl.FinanceControl.Domain.Interfaces.Services;
using FinanceControl.FinanceControl.Infraestructure.Persistence;
using FinanceControl.FinanceControl.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;
using Quartz;
using FinanceControl.FinanceControl.Application.Jobs;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog para logs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Adicionar serviços ao container
builder.Services.AddControllers();

// Configuração do DbContext 
builder.Services.AddDbContext<FinanceControlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()));



builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("RecurringTransactionJob");

    q.AddJob<RecurringTransactionJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("RecurringTransactionTrigger")
        .WithSimpleSchedule(x => x.WithIntervalInMinutes(1).RepeatForever())
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Configuração do AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
AutoMapperFactory.Initialize();

// Registrar repositórios e serviços
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<JwtService>();
// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();


// Adicionar o Swagger com suporte a autenticação JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Finance Control API",
        Version = "v1",
        Description = "API para controle financeiro pessoal",
        Contact = new OpenApiContact
        {
            Name = "Vitor Gutierrez",
            Email = "gutierrezvitor@gmail.com"
        }
    });

    // Adiciona suporte para JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Habilita a documentação XML
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddAuthorization();


var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Finance Control API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Usar o middleware de exceções
app.UseMiddleware<ExceptionMiddleware>();

//app.UseHangfireDashboard("/hangfire");

app.MapControllers();

app.Run();