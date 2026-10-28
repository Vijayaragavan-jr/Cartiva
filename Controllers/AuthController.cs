using Cartiva.Models;
using Cartiva.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthRequest request)
    {
        var result = await _authService.Register(request);

        if (result == "Email already registered")
            return Conflict(new { message = result });

        return Ok(new { message = result });
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.Login(request);

        if (result == null)
            return Unauthorized(
                new { message = "Invalid email or password" }
            );

        return Ok(new
        {
            message = "Login successful",
            accessToken = result.Value.AccessToken,
            refreshToken = result.Value.RefreshToken
        });
    }

    // LOGOUT
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        RefreshTokenRequest request)
    {
        var revoked = await _authService.RevokeRefreshToken(
            request.RefreshToken
        );

        if (!revoked)
        {
            return BadRequest(
                new
                {
                    message =
                        "Invalid or already revoked refresh token"
                }
            );
        }

        return Ok(new
        {
            message = "Logout successful"
        });
    }
}