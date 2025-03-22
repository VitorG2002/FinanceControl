-- Verifica se o banco de dados FinanceControlDb já existe, se não, cria.
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FinanceControlDb')
BEGIN
    CREATE DATABASE FinanceControlDb;
END
GO

USE FinanceControlDb;
GO

-- Verifica se o login finance_user existe, se não, cria.
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'finance_user')
BEGIN
    CREATE LOGIN finance_user WITH PASSWORD = 'Bingus123';
    CREATE USER finance_user FOR LOGIN finance_user;
    ALTER ROLE db_owner ADD MEMBER finance_user;
END
GO