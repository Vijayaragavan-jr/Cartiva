using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class UserService
{
    public async Task<List<object>> GetAllUsers()
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new
            {
                id = x.Id,
                username = x.Username,
                email = x.Email,
                createdAt = x.CreatedAt
            })
            .Cast<object>()
            .ToListAsync();
    }

    public async Task<object?> GetUserById(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                id = x.Id,
                username = x.Username,
                email = x.Email,
                createdAt = x.CreatedAt
            })
            .FirstOrDefaultAsync();
    }
}