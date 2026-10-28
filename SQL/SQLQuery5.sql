USE AuthHubDB;

CREATE TABLE Carts
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Carts_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT UQ_Carts_User
        UNIQUE (UserId)
);

CREATE TABLE CartItems
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,

    CONSTRAINT FK_CartItems_Carts
        FOREIGN KEY (CartId)
        REFERENCES Carts(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_CartItems_Products
        FOREIGN KEY (ProductId)
        REFERENCES Products(Id),

    CONSTRAINT UQ_CartItems_Cart_Product
        UNIQUE (CartId, ProductId),

    CONSTRAINT CK_CartItems_Quantity
        CHECK (Quantity > 0)
);