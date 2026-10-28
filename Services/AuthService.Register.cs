using Cartiva.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class AuthService
{
    // REGISTER
    public async Task<string> Register(AuthRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (existingUser != null)
            return "Email already registered";

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return "User registered successfully";
    }
}