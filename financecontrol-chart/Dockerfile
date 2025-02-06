# Usar a imagem base do SQL Server
FROM mcr.microsoft.com/mssql/server:2019-latest

# Instalar dependências
USER root
RUN apt-get update && \
    ACCEPT_EULA=Y apt-get install -y \
    mssql-tools \
    unixodbc-dev \
    && rm -rf /var/lib/apt/lists/*

# Definir o usuário para não ser root
USER mssql

# Definir o comando inicial (apenas para fins de exemplo)
CMD ["/opt/mssql/bin/sqlservr"]
