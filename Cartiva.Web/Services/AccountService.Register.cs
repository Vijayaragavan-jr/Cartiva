using System.Net.Http.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class AccountService
{
    public async Task<bool> RegisterAsync(
        RegisterViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient("CartivaAPI");

        var registerRequest = new
        {
            username = model.Username,
            email = model.Email,
            password = model.Password
        };

        var response = await client.PostAsJsonAsync(
            "http://localhost:8080/api/auth/register",
            registerRequest
        );

        return response.IsSuccessStatusCode;
    }
}