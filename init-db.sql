-- Cria o banco de dados se não existir
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FinanceControlDb')
BEGIN
    CREATE DATABASE FinanceControlDb;
END
GO

-- Usa o banco de dados recém-criado
USE FinanceControlDb;
GO

-- Cria o usuário apenas se não existir
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'finance_user')
BEGIN
    CREATE LOGIN finance_user WITH PASSWORD = 'Bingus123';
    CREATE USER finance_user FOR LOGIN finance_user;
    ALTER ROLE db_owner ADD MEMBER finance_user;
END
GO
