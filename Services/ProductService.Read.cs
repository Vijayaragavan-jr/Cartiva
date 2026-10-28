using Cartiva.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class ProductService
{
    // GET ALL PRODUCTS
    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    // GET PRODUCT BY ID
    public async Task<Product?> GetProductById(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}