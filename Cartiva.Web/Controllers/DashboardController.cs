using Cartiva.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Web.Controllers;

public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    // DASHBOARD
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _dashboardService.GetDashboardAsync();

        return View(model);
    }
}