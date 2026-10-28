using Cartiva.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Web.Controllers;

public class CartController : Controller
{
    private readonly CartService _cartService;

    public CartController(CartService cartService)
    {
        _cartService = cartService;
    }

    // VIEW CART
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cart = await _cartService.GetCartAsync();

        return View(cart);
    }

    // UPDATE CART QUANTITY
    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(
        int productId,
        int quantity)
    {
        await _cartService.UpdateQuantityAsync(
            productId,
            quantity
        );

        return RedirectToAction("Index");
    }

    // REMOVE PRODUCT FROM CART
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(
        int productId)
    {
        await _cartService.RemoveFromCartAsync(productId);

        return RedirectToAction("Index");
    }
}