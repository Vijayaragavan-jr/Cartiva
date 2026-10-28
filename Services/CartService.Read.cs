using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class CartService
{
    // GET CART
    public async Task<object?> GetCart(int userId)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
            return null;

        var items = await _context.CartItems
            .Where(x => x.CartId == cart.Id)
            .Join(
                _context.Products,
                item => item.ProductId,
                product => product.Id,
                (item, product) => new
                {
                    productId = product.Id,
                    name = product.Name,
                    price = product.Price,
                    quantity = item.Quantity,
                    total = product.Price * item.Quantity
                }
            )
            .ToListAsync();

        return new
        {
            cartId = cart.Id,
            items,
            grandTotal = items.Sum(x => x.total)
        };
    }
}