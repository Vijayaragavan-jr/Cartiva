using Cartiva.Web.Models;
using Cartiva.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Web.Controllers;

public class ProductController : Controller
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProductsAsync();

        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel product)
    {
        if (!ModelState.IsValid)
        {
            var products = await _productService.GetProductsAsync();

            ViewBag.ShowCreateForm = true;

            return View("Index", products);
        }

        var success =
            await _productService.CreateProductAsync(product);

        if (success)
        {
            TempData["SuccessMessage"] =
                "Product created successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "Failed to create product.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Update(
        int id,
        ProductViewModel product)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] =
                "Please enter valid product details.";

            return RedirectToAction(nameof(Index));
        }

        var success =
            await _productService.UpdateProductAsync(
                id,
                product
            );

        if (success)
        {
            TempData["SuccessMessage"] =
                "Product updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "Failed to update product.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var success =
            await _productService.DeleteProductAsync(id);

        if (success)
        {
            TempData["SuccessMessage"] =
                "Product deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "Failed to delete product.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] =
                "Please select a CSV file.";

            return RedirectToAction(nameof(Index));
        }

        var success =
            await _productService.UploadProductsCsvAsync(file);

        if (success)
        {
            TempData["SuccessMessage"] =
                "CSV products uploaded successfully.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "CSV upload failed.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var success =
            await _productService.AddToCartAsync(productId);

        if (success)
        {
            TempData["SuccessMessage"] =
                "Product added to cart.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "Failed to add product to cart.";
        }

        return RedirectToAction(nameof(Index));
    }
}