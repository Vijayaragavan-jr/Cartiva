using Cartiva.Models;
using Cartiva.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class RefreshTokenController : ControllerBase
{
    private readonly AuthService _authService;

    public RefreshTokenController(AuthService authService)
    {
        _authService = authService;
    }

    // REFRESH ACCESS TOKEN
    [HttpPost]
    public async Task<IActionResult> Refresh(
        RefreshTokenRequest request)
    {
        var result = await _authService.RefreshAccessToken(
            request.RefreshToken
        );

        if (result == null)
        {
            return Unauthorized(
                new { message = "Invalid or expired refresh token" }
            );
        }

        return Ok(new
        {
            message = "Access token refreshed successfully",
            accessToken = result
        });
    }
}