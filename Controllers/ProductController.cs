using Cartiva.Models;
using Cartiva.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    // CREATE PRODUCT — ADMIN ONLY
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Product product)
    {
        var productId = await _productService.CreateProduct(product);

        return Ok(new
        {
            message = "Product created successfully",
            id = productId
        });
    }

    // GET ALL PRODUCTS — AUTHENTICATED USERS
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAllProducts());
    }

    // GET PRODUCT BY ID — AUTHENTICATED USERS
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductById(id);

        if (product == null)
            return NotFound(new
            {
                message = "Product not found"
            });

        return Ok(product);
    }

    // UPDATE PRODUCT — ADMIN ONLY
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        var updated = await _productService.UpdateProduct(id, product);

        if (!updated)
            return NotFound(new
            {
                message = "Product not found"
            });

        return Ok(new
        {
            message = "Product updated successfully"
        });
    }

    // DELETE PRODUCT — ADMIN ONLY
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteProduct(id);

        if (!deleted)
            return NotFound(new
            {
                message = "Product not found"
            });

        return Ok(new
        {
            message = "Product deleted successfully"
        });
    }

    // BULK CSV UPLOAD — ADMIN ONLY
    [HttpPost("bulk-upload")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> BulkUpload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new
            {
                message = "Please upload a CSV file"
            });
        }

        if (!Path.GetExtension(file.FileName)
            .Equals(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "Only CSV files are allowed"
            });
        }

        var result = await _productService.BulkUploadProducts(file);

        return Ok(result);
    }
}