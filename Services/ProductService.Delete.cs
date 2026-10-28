using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class ProductService
{
    // DELETE PRODUCT
    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}