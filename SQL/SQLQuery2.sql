USE AuthHubDB;
GO

UPDATE Users
SET PasswordHash = 'h3bxCOJHqx4rMjBCwEnCZkB8gfutQb3h6N/Bu2b9Jn4='
WHERE Email = 'vijay@authhub.com';
GO