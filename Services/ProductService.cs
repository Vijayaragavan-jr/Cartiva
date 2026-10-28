using Cartiva.Data;

namespace Cartiva.Services;

public partial class ProductService
{
    private readonly CartivaDbContext _context;

    public ProductService(CartivaDbContext context)
    {
        _context = context;
    }
}