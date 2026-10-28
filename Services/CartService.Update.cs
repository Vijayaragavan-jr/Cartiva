using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class CartService
{
    // UPDATE QUANTITY
    public async Task<bool> UpdateQuantity(
        int userId,
        int productId,
        int quantity)
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

        if (quantity <= 0)
            _context.CartItems.Remove(item);
        else
            item.Quantity = quantity;

        await _context.SaveChangesAsync();

        return true;
    }
}