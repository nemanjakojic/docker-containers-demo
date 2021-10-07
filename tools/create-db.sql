-- ======================================== --
-- Script parameters
-- ======================================== --

-- Create the SQL Server Login 'AppLogin'
CREATE LOGIN AppLogin WITH PASSWORD = 'TestPwd.098';  
GO  

-- Creates a database user for the login created above.  
CREATE USER AppUser FOR LOGIN AppLogin;  
GO

-- Creates a database role owned by the user created above
CREATE ROLE AppRole AUTHORIZATION AppUser
GO

-- Adds the database user to the new database role
ALTER ROLE AppRole ADD MEMBER AppUser
GO

-- Creates a new schema owned by the created database role
CREATE SCHEMA app AUTHORIZATION AppRole
GO

-- DB Schema Initialization
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
GO

-- Test data initialization
DECLARE @CurrentTime DATETIME = GETUTCDATE()

INSERT INTO app.Account(Username, PasswordHash, Created, CreatedBy, Modified, ModifiedBy)
VALUES
('nemanja.kojic@gmail.com', HASHBYTES('SHA2_512',CAST(N'TestPwd.1234' AS VARBINARY(MAX))+0xF1202F8A), @CurrentTime, 'nemanja', @CurrentTime, 'nemanja'),
('nkojic@gmail.com', HASHBYTES('SHA2_512',CAST(N'TestPwd.1234' AS VARBINARY(MAX))+0xF1202F8A), @CurrentTime, 'nemanja', @CurrentTime, 'nemanja')

GO

