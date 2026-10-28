using Cartiva.Data;

namespace Cartiva.Services;

public partial class UserService
{
    private readonly CartivaDbContext _context;

    public UserService(CartivaDbContext context)
    {
        _context = context;
    }
}