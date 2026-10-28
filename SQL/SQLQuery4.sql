USE AuthHubDB;
GO

SELECT
    Id,
    Username,
    Email,
    PasswordHash,
    CreatedAt
FROM Users;