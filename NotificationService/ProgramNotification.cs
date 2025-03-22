using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

// Registrar o NotificationService
builder.Services.AddScoped<INotificationEmailService, NotificationEmailService>();



// Registrar o RabbitMqListener como um servi�o em segundo plano
builder.Services.AddHostedService<RabbitMqListener>();

var host = builder.Build();
host.Run();