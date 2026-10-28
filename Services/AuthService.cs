using Cartiva.Data;
using Cartiva.Models;
using Microsoft.AspNetCore.Identity;

namespace Cartiva.Services;

public partial class AuthService
{
    private readonly CartivaDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(
        CartivaDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }
}