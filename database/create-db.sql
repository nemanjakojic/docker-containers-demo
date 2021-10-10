-- ======================================== --
-- Create database accounts_db
-- ======================================== --
USE master;

IF DB_ID('accounts_db') IS NULL
BEGIN
  CREATE DATABASE accounts_db;
END

GO 

-- ======================================== --
-- Create server login 'AppLogin'
-- ======================================== --
USE accounts_db

IF NOT EXISTS (
	SELECT 1 
	FROM master.sys.server_principals 
	WHERE [name] = N'AppLogin' and [type] IN ('C','E', 'G', 'K', 'S', 'U')
)
BEGIN
	-- Create the SQL Server Login 'AppLogin'
	CREATE LOGIN AppLogin WITH PASSWORD = 'TestPwd.098';
END

GO  

-- ======================================================= --
-- Create database user 'AppUser' for the login 'AppLogin'
-- ======================================================= --
USE accounts_db

IF NOT EXISTS (
	SELECT 1 
	FROM sys.database_principals
	WHERE [name] = N'AppUser' and [type] IN ('C','E', 'G', 'K', 'S', 'U')
)
BEGIN
	CREATE USER AppUser FOR LOGIN AppLogin;
END

GO

-- ======================================== --
-- Create database role 'AppRole'
-- ======================================== --
USE accounts_db

IF NOT EXISTS (
	SELECT 1 
	FROM sys.database_principals 
	WHERE [name] = N'AppRole' and Type = 'R'
)
BEGIN
	CREATE ROLE AppRole
END

GO

-- ===================================================== --
-- Add the user 'AppUser' to the database role 'AppRole'
-- ===================================================== --
USE accounts_db

ALTER ROLE AppRole ADD MEMBER AppUser

GO

-- ========================================================= --
-- Create schema 'app' owned by the role principal 'AppRole'
-- ========================================================= --
USE accounts_db

IF NOT EXISTS(
	SELECT 1 
	FROM sys.schemas
	WHERE name = 'app'
)
BEGIN
	EXEC('CREATE SCHEMA app AUTHORIZATION AppRole')
END

GO

-- ========================================================= --
-- Create table 'app.Account' with accompanying indexes
-- ========================================================= --
USE accounts_db 

IF NOT EXISTS (
	SELECT * 
	FROM sys.tables t
	JOIN sys.schemas s ON t.schema_id = s.schema_id
	WHERE s.name = N'app' AND t.name = N'Account' and t.type = 'U'
)
BEGIN
	CREATE TABLE app.Account(
		Id BIGINT PRIMARY KEY IDENTITY(1, 1),
		Username NVARCHAR(100) NOT NULL UNIQUE,
		PasswordHash VARBINARY(MAX) NOT NULL,
		Created DATETIME NOT NULL,
		CreatedBy NVARCHAR(100) NOT NULL,
		Modified DATETIME NOT NULL,
		ModifiedBy NVARCHAR(100) NOT NULL
	)
	
	CREATE NONCLUSTERED INDEX UsernameIdx ON app.Account(Username);
END

GO

-- ========================================================= --
-- Insert test data
-- ========================================================= --
USE accounts_db

-- Test data initialization
DECLARE @CurrentTime DATETIME = GETUTCDATE()
DECLARE @TestUser NVARCHAR(100) = 'testuser@email.com'

IF NOT EXISTS (
	SELECT 1 
	FROM app.Account
	WHERE Username = @TestUser
)
BEGIN
	INSERT INTO app.Account(Username, PasswordHash, Created, CreatedBy, Modified, ModifiedBy)
	VALUES
	(@TestUser, 0x243261243130247436585a49467456722f6862766e624a3646567932754e63484a73567733324c692e6453797237362e36414352565343323453454b, @CurrentTime, 'app', @CurrentTime, 'app')
END

GO

