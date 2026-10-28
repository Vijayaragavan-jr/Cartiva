using Cartiva.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class CartService
{
    // ADD TO CART
    public async Task<object> AddToCart(
        int userId,
        int productId,
        int quantity)
    {
        if (quantity <= 0)
        {
            return new
            {
                success = false,
                message = "Quantity must be greater than zero"
            };
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product == null)
        {
            return new
            {
                success = false,
                message = "Product not found"
            };
        }

        var cart = await _context.Carts
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var item = await _context.CartItems
            .FirstOrDefaultAsync(
                x => x.CartId == cart.Id &&
                     x.ProductId == productId
            );

        if (item == null)
        {
            _context.CartItems.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity += quantity;
        }

        await _context.SaveChangesAsync();

        return new
        {
            success = true,
            message = "Product added to cart"
        };
    }
}