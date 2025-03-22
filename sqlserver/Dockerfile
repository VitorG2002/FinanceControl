FROM mcr.microsoft.com/mssql/server:2019-latest

# Trocar para root temporariamente
USER 0

# Atualizar pacotes e instalar dependências essenciais
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    curl \
    apt-transport-https \
    ca-certificates \
    gnupg \
    gnupg2 \
    gnupg1 \
    unixodbc-dev && \
    curl -sSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    echo "deb [arch=amd64] https://packages.microsoft.com/ubuntu/20.04/prod focal main" > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y --no-install-recommends mssql-tools && \
    rm -rf /var/lib/apt/lists/*

# Adicionar mssql-tools ao PATH
RUN echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> /etc/bash.bashrc && \
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

# Voltar para usuário padrão do SQL Server
USER 10001

# Rodar o SQL Server
CMD ["/opt/mssql/bin/sqlservr"]
