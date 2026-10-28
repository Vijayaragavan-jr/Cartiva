using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class UserService
{
    public async Task<List<UserViewModel>> GetUsersAsync()
    {
        var client =
            _httpClientFactory.CreateClient("CartivaAPI");

        var token = _httpContextAccessor.HttpContext?
            .Session.GetString("AccessToken");

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
        }

        var response =
            await client.GetAsync("http://localhost:8080/api/user");

        if (!response.IsSuccessStatusCode)
        {
            return new List<UserViewModel>();
        }

        var users =
            await response.Content
                .ReadFromJsonAsync<List<UserViewModel>>();

        return users ??
               new List<UserViewModel>();
    }
}