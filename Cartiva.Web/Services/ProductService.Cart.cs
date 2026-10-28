using System.Net.Http.Headers;

namespace Cartiva.Web.Services;

public partial class ProductService
{
    public async Task<bool> AddToCartAsync(
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

        var response = await client.PostAsync(
            $"http://localhost:8080/api/cart/add?productId={productId}&quantity=1",
            null
        );

        return response.IsSuccessStatusCode;
    }
}