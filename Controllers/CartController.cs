using Cartiva.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Cartiva.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );
    }

    // ADD TO CART
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(
        [FromQuery, Range(
            1,
            int.MaxValue,
            ErrorMessage = "Product ID must be greater than zero"
        )]
        int productId,

        [FromQuery, Range(
            1,
            int.MaxValue,
            ErrorMessage = "Quantity must be greater than zero"
        )]
        int quantity)
    {
        var result = await _cartService.AddToCart(
            GetUserId(),
            productId,
            quantity
        );

        return Ok(result);
    }

    // GET CART
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var cart = await _cartService.GetCart(
            GetUserId()
        );

        if (cart == null)
        {
            return Ok(new
            {
                message = "Cart is empty",
                items = Array.Empty<object>(),
                grandTotal = 0
            });
        }

        return Ok(cart);
    }

    // UPDATE QUANTITY
    [HttpPut("update")]
    public async Task<IActionResult> UpdateQuantity(
        [FromQuery, Range(
            1,
            int.MaxValue,
            ErrorMessage = "Product ID must be greater than zero"
        )]
        int productId,

        [FromQuery, Range(
            1,
            int.MaxValue,
            ErrorMessage = "Quantity must be greater than zero"
        )]
        int quantity)
    {
        var updated = await _cartService.UpdateQuantity(
            GetUserId(),
            productId,
            quantity
        );

        if (!updated)
        {
            return NotFound(
                new { message = "Cart item not found" }
            );
        }

        return Ok(new
        {
            message = "Cart quantity updated successfully"
        });
    }

    // REMOVE FROM CART
    [HttpDelete("remove/{productId}")]
    public async Task<IActionResult> RemoveFromCart(
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Product ID must be greater than zero"
        )]
        int productId)
    {
        var removed = await _cartService.RemoveFromCart(
            GetUserId(),
            productId
        );

        if (!removed)
        {
            return NotFound(
                new { message = "Cart item not found" }
            );
        }

        return Ok(new
        {
            message = "Product removed from cart"
        });
    }
}