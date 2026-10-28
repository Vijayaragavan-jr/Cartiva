using Cartiva.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cartiva.Services;

public partial class AuthService
{
    // REFRESH ACCESS TOKEN
    public async Task<string?> RefreshAccessToken(
        string refreshToken)
    {
        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (storedRefreshToken == null)
            return null;

        if (
            storedRefreshToken.IsRevoked ||
            storedRefreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Id == storedRefreshToken.UserId
            );

        if (user == null)
            return null;

        var claims = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()
            ),
            new Claim(
                JwtRegisteredClaimNames.Email,
                user.Email
            ),
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()
            ),
            new Claim(
                ClaimTypes.Name,
                user.Username
            )
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var expirationMinutes = Convert.ToDouble(
            _configuration["Jwt:AccessTokenExpirationMinutes"]
        );

        var accessToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                expirationMinutes
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(accessToken);
    }

    // LOGOUT
    public async Task<bool> RevokeRefreshToken(
        string refreshToken)
    {
        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(
                x =>
                    x.Token == refreshToken &&
                    !x.IsRevoked
            );

        if (storedRefreshToken == null)
            return false;

        storedRefreshToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        return true;
    }
}