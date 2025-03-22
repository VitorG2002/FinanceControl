# Usa a imagem base do .NET para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Usa a imagem do SDK para build e execução das migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto FinanceControl e NotificationService
COPY ./FinanceControl/FinanceControl.csproj ./FinanceControl/
COPY ./NotificationService/NotificationService.csproj ./NotificationService/

# Restaura as dependências
WORKDIR /src/FinanceControl
RUN sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN dotnet restore

WORKDIR /src/NotificationService
RUN dotnet restore

# Copia todo o código-fonte
COPY . . 
# Publica os projetos individualmente para evitar conflitos
RUN dotnet publish "FinanceControl/FinanceControl.csproj" -c Release -o /app/publish/FinanceControl
RUN dotnet publish "NotificationService/NotificationService.csproj" -c Release -o /app/publish/NotificationService

# Copia os binários para a imagem final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish/FinanceControl .
COPY --from=build /app/publish/NotificationService ./notification

# Para rodar as migrations na inicialização
ENTRYPOINT ["dotnet", "FinanceControl.dll"]
