using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class AccountService
{
    public async Task<LoginResponse?> LoginAsync(LoginViewModel model)
    {
        var client = _httpClientFactory.CreateClient("CartivaAPI");

        var loginRequest = new
        {
            email = model.Email,
            password = model.Password
        };

        var response = await client.PostAsJsonAsync(
            "http://localhost:8080/api/auth/login", loginRequest);

        if (!response.IsSuccessStatusCode)
            return null;

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginResponse == null || string.IsNullOrWhiteSpace(loginResponse.AccessToken))
            return null;

        var httpContext = _httpContextAccessor.HttpContext;

        httpContext?.Session.SetString("AccessToken", loginResponse.AccessToken);
        httpContext?.Session.SetString("RefreshToken", loginResponse.RefreshToken);

        var role = GetRoleFromToken(loginResponse.AccessToken);
        httpContext?.Session.SetString("UserRole", role);

        return loginResponse;
    }

    private static string GetRoleFromToken(string token)
    {
        var parts = token.Split('.');

        if (parts.Length != 3)
            return "Customer";

        var payload = parts[1]
            .Replace('-', '+')
            .Replace('_', '/');

        payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

        var payloadBytes = Convert.FromBase64String(payload);

        using var document = JsonDocument.Parse(payloadBytes);

        var root = document.RootElement;

        var roleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        if (root.TryGetProperty(roleClaim, out var roleValue))
        {
            if (roleValue.ValueKind == JsonValueKind.String)
                return roleValue.GetString() ?? "Customer";

            if (roleValue.ValueKind == JsonValueKind.Array && roleValue.GetArrayLength() > 0)
                return roleValue[0].GetString() ?? "Customer";
        }

        if (root.TryGetProperty("role", out var shortRole) &&
            shortRole.ValueKind == JsonValueKind.String)
        {
            return shortRole.GetString() ?? "Customer";
        }

        return "Customer";
    }
}

public class LoginResponse
{
    public string Message { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
