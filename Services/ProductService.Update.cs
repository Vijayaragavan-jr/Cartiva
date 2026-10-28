using Cartiva.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class ProductService
{
    // UPDATE PRODUCT
    public async Task<bool> UpdateProduct(int id, Product product)
    {
        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (existingProduct == null)
            return false;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;

        await _context.SaveChangesAsync();

        return true;
    }
}