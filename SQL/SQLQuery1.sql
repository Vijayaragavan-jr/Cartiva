-- =============================================
-- CREATE DATABASE
-- =============================================

CREATE DATABASE AuthHubDB;
GO

USE AuthHubDB;
GO


-- =============================================
-- USERS TABLE
-- =============================================

CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO


-- =============================================
-- REFRESH TOKENS TABLE
-- =============================================

CREATE TABLE RefreshTokens
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    IsRevoked BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_RefreshTokens_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);
GO


-- =============================================
-- PRODUCTS TABLE
-- =============================================

CREATE TABLE Products
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(10,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO


-- =============================================
-- INSERT CLEAN USERS
-- =============================================

INSERT INTO Users
(
    Username,
    Email,
    PasswordHash
)
VALUES
(
    'Vijay',
    'vijay@authhub.com',
    'PLACEHOLDER'
);
GO


-- =============================================
-- INSERT CLEAN PRODUCTS
-- =============================================

INSERT INTO Products
(
    Name,
    Description,
    Price
)
VALUES
(
    'Wireless Keyboard',
    'Bluetooth mechanical keyboard',
    2499.00
),
(
    'Wireless Mouse',
    'Ergonomic wireless mouse',
    1499.00
),
(
    'USB-C Hub',
    'Multi-port USB-C connectivity hub',
    1999.00
);
GO