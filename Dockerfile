FROM mcr.microsoft.com/mssql/server:2019-latest

# Instalar dependências (mssql-tools, unixodbc-dev)
USER root  
RUN apt-get update && ACCEPT_EULA=Y apt-get install -y \
    mssql-tools \
    unixodbc-dev \
    && rm -rf /var/lib/apt/lists/*

# Definir variáveis de ambiente (opcional)
ENV PATH="$PATH:/opt/mssql-tools/bin"

# Voltar para o usuário padrão do SQL Server
USER mssql

# Definir o comando de inicialização do SQL Server
CMD ["/opt/mssql/bin/sqlservr"]
