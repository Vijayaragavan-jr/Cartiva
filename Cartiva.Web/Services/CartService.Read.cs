using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class CartService
{
    public async Task<CartViewModel> GetCartAsync()
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
            await client.GetAsync(
                "http://localhost:8080/api/cart");

        if (!response.IsSuccessStatusCode)
        {
            return new CartViewModel();
        }

        var cart =
            await response.Content
                .ReadFromJsonAsync<CartViewModel>();

        return cart ?? new CartViewModel();
    }

    public async Task<bool> UpdateQuantityAsync(
        int productId,
        int quantity)
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

        var response = await client.PutAsync(
            $"api/cart/update?productId={productId}&quantity={quantity}",
            null
        );

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveFromCartAsync(
        int productId)
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

        var response = await client.DeleteAsync(
            $"api/cart/remove/{productId}"
        );

        return response.IsSuccessStatusCode;
    }
}