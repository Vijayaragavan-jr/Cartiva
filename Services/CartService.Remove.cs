using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class CartService
{
    // REMOVE FROM CART
    public async Task<bool> RemoveFromCart(
        int userId,
        int productId)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
            return false;

        var item = await _context.CartItems
            .FirstOrDefaultAsync(
                x => x.CartId == cart.Id &&
                     x.ProductId == productId
            );

        if (item == null)
            return false;

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }
}