using Cartiva.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Web.Controllers;

public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetUsersAsync();

        return View(users);
    }
}