using Cartiva.Data;

namespace Cartiva.Services;

public partial class CartService
{
    private readonly CartivaDbContext _context;

    public CartService(CartivaDbContext context)
    {
        _context = context;
    }
}