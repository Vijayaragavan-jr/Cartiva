USE AuthHubDB;

SELECT
    u.Id AS UserId,
    u.Username,
    u.Email,
    u.CreatedAt AS RegisteredAt,
    c.Id AS CartId,
    p.Id AS ProductId,
    p.Name AS ProductName,
    p.Price,
    ci.Quantity,
    p.Price * ci.Quantity AS ItemTotal,
    c.CreatedAt AS CartCreatedAt
FROM Users u
LEFT JOIN Carts c
    ON u.Id = c.UserId
LEFT JOIN CartItems ci
    ON c.Id = ci.CartId
LEFT JOIN Products p
    ON ci.ProductId = p.Id

ORDER BY u.Id, p.Id;