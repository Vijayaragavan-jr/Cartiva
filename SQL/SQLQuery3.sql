USE AuthHubDB;
GO

SELECT Id, Username, Email, CreatedAt
FROM Users;

SELECT Id, Name, Description, Price, CreatedAt
FROM Products
ORDER BY Id;