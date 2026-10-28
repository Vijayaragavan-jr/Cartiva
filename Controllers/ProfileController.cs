using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cartiva.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class ProfileController : ControllerBase
{
    // GET PROFILE
    [HttpGet]
    [Authorize]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        var email = User.FindFirstValue(
            ClaimTypes.Email
        );

        var username = User.FindFirstValue(
            ClaimTypes.Name
        );

        return Ok(new
        {
            message = "You are authorized",
            userId,
            email,
            username
        });
    }
}