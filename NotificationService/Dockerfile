# Usa a imagem base do .NET para rodar a aplicação
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Usa a imagem do SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo do projeto
COPY ./NotificationService/NotificationService.csproj ./NotificationService/

# Restaura as dependências
RUN dotnet restore ./NotificationService/NotificationService.csproj

# Copia o restante do código-fonte para o container
COPY . ./


# Executa o build e publica
RUN dotnet build ./NotificationService/NotificationService.csproj -c Release
RUN dotnet publish ./NotificationService/NotificationService.csproj -c Release -o /app/publish


# Copia os binários para a imagem final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Define o entrypoint para rodar o Worker Service
ENTRYPOINT ["dotnet", "NotificationService.dll"]
