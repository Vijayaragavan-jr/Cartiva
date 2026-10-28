using Cartiva.Web.Models;
using Cartiva.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Web.Controllers;

public class AccountController : Controller
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var loginResponse = await _accountService.LoginAsync(model);

        if (loginResponse == null)
        {
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View(model);
        }

        var role = HttpContext.Session.GetString("UserRole");

        if (string.Equals(role, "Admin",
            StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Index", "Admin");
        }

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var success = await _accountService.RegisterAsync(model);

        if (!success)
        {
            ViewBag.ErrorMessage = "Registration failed. Please try again.";
            return View(model);
        }

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}