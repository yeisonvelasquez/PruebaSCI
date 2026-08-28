IF DB_ID(N'PruebaSCI') IS NULL
BEGIN
    CREATE DATABASE PruebaSCI;
END
GO

USE PruebaSCI;
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        Id int IDENTITY(1, 1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        Name nvarchar(200) NOT NULL,
        Description nvarchar(1000) NOT NULL,
        Price decimal(18, 2) NOT NULL CONSTRAINT CK_Products_Price_Positive CHECK (Price > 0),
        CreatedDate datetime NOT NULL CONSTRAINT DF_Products_CreatedDate DEFAULT GETUTCDATE()
    );
END
GO

CREATE OR ALTER PROCEDURE dbo.Product_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Description, Price, CreatedDate
    FROM dbo.Products
    ORDER BY Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.Product_GetById
    @Id int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Description, Price, CreatedDate
    FROM dbo.Products
    WHERE Id = @Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.Product_Create
    @Name nvarchar(200),
    @Description nvarchar(1000),
    @Price decimal(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Products (Name, Description, Price)
    OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Description, INSERTED.Price, INSERTED.CreatedDate
    VALUES (@Name, @Description, @Price);
END
GO

CREATE OR ALTER PROCEDURE dbo.Product_Update
    @Id int,
    @Name nvarchar(200),
    @Description nvarchar(1000),
    @Price decimal(18, 2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Products
    SET Name = @Name, Description = @Description, Price = @Price
    WHERE Id = @Id;
    SELECT CONVERT(int, @@ROWCOUNT);
END
GO

CREATE OR ALTER PROCEDURE dbo.Product_Delete
    @Id int
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Products WHERE Id = @Id;
    SELECT CONVERT(int, @@ROWCOUNT);
END
GO
