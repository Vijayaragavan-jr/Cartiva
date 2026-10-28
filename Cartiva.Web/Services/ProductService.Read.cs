using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class ProductService
{
    public async Task<List<ProductViewModel>> GetProductsAsync()
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

        var response = await client.GetAsync(
     "http://localhost:8080/api/product"
 );

        if (!response.IsSuccessStatusCode)
        {
            return new List<ProductViewModel>();
        }

        var products =
            await response.Content
                .ReadFromJsonAsync<List<ProductViewModel>>();

        return products ??
               new List<ProductViewModel>();
    }
}