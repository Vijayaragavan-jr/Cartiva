using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class DashboardService
{
    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        var model = new DashboardViewModel();

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

        var productResponse =
            await client.GetAsync("http://localhost:8080/api/product");

        if (productResponse.IsSuccessStatusCode)
        {
            var products =
                await productResponse.Content
                    .ReadFromJsonAsync<List<ProductViewModel>>();

            if (products != null)
            {
                model.TotalProducts = products.Count;
            }
        }

        var cartResponse =
            await client.GetAsync("api/cart");

        if (cartResponse.IsSuccessStatusCode)
        {
            var cart =
                await cartResponse.Content
                    .ReadFromJsonAsync<CartViewModel>();

            if (cart != null)
            {
                model.TotalCarts = 1;
                model.TotalCartItems = cart.Items.Count;
                model.CartTotal = cart.GrandTotal;
            }
        }

        return model;
    }
}