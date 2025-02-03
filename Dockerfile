# Usa a imagem base do .NET
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Usa a imagem do SDK para build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FinanceControl.API/FinanceControl.API.csproj", "FinanceControl.API/"]
COPY ["FinanceControl.Application/FinanceControl.Application.csproj", "FinanceControl.Application/"]
COPY ["FinanceControl.Domain/FinanceControl.Domain.csproj", "FinanceControl.Domain/"]
COPY ["FinanceControl.Infrastructure/FinanceControl.Infrastructure.csproj", "FinanceControl.Infrastructure/"]
RUN dotnet restore "FinanceControl.API/FinanceControl.API.csproj"
COPY . .
WORKDIR "/src/FinanceControl.API"
RUN dotnet build "FinanceControl.API.csproj" -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish "FinanceControl.API.csproj" -c Release -o /app/publish

# Copia os arquivos publicados para a imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceControl.API.dll"]