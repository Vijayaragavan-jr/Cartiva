
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Cartiva.Web.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    private HttpClient GetApiClient()
    {
        var client = _httpClientFactory.CreateClient("CartivaAPI");
        var token = HttpContext.Session.GetString("AccessToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    private bool IsAdmin() =>
        string.Equals(
            HttpContext.Session.GetString("UserRole"),
            "Admin",
            StringComparison.OrdinalIgnoreCase);

    [HttpGet("dashboard")]
    public async Task<IActionResult> Index()
    {
        if (!IsAdmin())
            return RedirectToAction("Login", "Account");

        var response = await GetApiClient().GetAsync(
            "http://localhost:5197/api/product");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.ErrorMessage = "Unable to load products.";
            return View(new List<JsonElement>());
        }

        var products = await response.Content
            .ReadFromJsonAsync<List<JsonElement>>();

        return View(products ?? new List<JsonElement>());
    }

    [HttpGet("export-products")]
    public async Task<IActionResult> ExportProducts()
    {
        if (!IsAdmin())
            return RedirectToAction("Login", "Account");

        var response = await GetApiClient().GetAsync(
            "http://localhost:8080/api/product");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Unable to export products.";
            return RedirectToAction(nameof(Index));
        }

        var products = await response.Content
            .ReadFromJsonAsync<List<JsonElement>>();

        var csv = new StringBuilder();
        csv.AppendLine("Id,Name,Description,Price");

        foreach (var product in products ?? new List<JsonElement>())
        {
            var id = GetPropertyValue(product, "id");
            var name = GetPropertyValue(product, "name");
            var description = GetPropertyValue(product, "description");
            var price = GetPropertyValue(product, "price");

            csv.AppendLine(
                $"{EscapeCsv(id)},{EscapeCsv(name)},{EscapeCsv(description)},{EscapeCsv(price)}");
        }

        var fileBytes = Encoding.UTF8.GetPreamble()
            .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
            .ToArray();

        return File(
            fileBytes,
            "text/csv; charset=utf-8",
            $"Cartiva_Products_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
    }

    private static string GetPropertyValue(
        JsonElement product,
        string propertyName)
    {
        if (!product.TryGetProperty(propertyName, out var value))
            return string.Empty;

        if (value.ValueKind == JsonValueKind.Null ||
            value.ValueKind == JsonValueKind.Undefined)
            return string.Empty;

        if (value.ValueKind == JsonValueKind.Number &&
            value.TryGetDecimal(out var number))
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        return value.ToString();
    }

    private static string EscapeCsv(string value)
    {
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        string name,
        string description,
        decimal price)
    {
        if (!IsAdmin())
            return Forbid();

        var response = await GetApiClient().PostAsJsonAsync(
            "http://localhost:8080/api/product",
            new { name, description, price });

        if (!response.IsSuccessStatusCode)
            TempData["Error"] = "Product creation failed.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(
        int id,
        string name,
        string description,
        decimal price)
    {
        if (!IsAdmin())
            return Forbid();

        var response = await GetApiClient().PutAsJsonAsync(
            $"http://localhost:8080/api/product/{id}",
            new { id, name, description, price });

        if (!response.IsSuccessStatusCode)
            TempData["Error"] = "Product update failed.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!IsAdmin())
            return Forbid();

        var response = await GetApiClient()
            .DeleteAsync($"http://localhost:8080/api/product/{id}");

        if (!response.IsSuccessStatusCode)
            TempData["Error"] = "Product deletion failed.";

        return RedirectToAction(nameof(Index));
    }
}
