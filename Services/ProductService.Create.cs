using Cartiva.Models;

namespace Cartiva.Services;

public partial class ProductService
{
    // CREATE PRODUCT
    public async Task<int> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product.Id;
    }
}