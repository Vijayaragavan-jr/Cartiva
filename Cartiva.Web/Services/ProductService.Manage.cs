using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cartiva.Web.Models;

namespace Cartiva.Web.Services;

public partial class ProductService
{
    // CREATE PRODUCT
    public async Task<bool> CreateProductAsync(ProductViewModel product)
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

        var response = await client.PostAsJsonAsync(
            "http://localhost:8080/api/product",
            new
            {
                name = product.Name,
                description = product.Description,
                price = product.Price
            }
        );

        return response.IsSuccessStatusCode;
    }

    // UPDATE PRODUCT
    public async Task<bool> UpdateProductAsync(
        int id,
        ProductViewModel product)
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

        var response = await client.PutAsJsonAsync(
            $"api/product/{id}",
            new
            {
                name = product.Name,
                description = product.Description,
                price = product.Price
            }
        );

        return response.IsSuccessStatusCode;
    }

    // DELETE PRODUCT
    public async Task<bool> DeleteProductAsync(int id)
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
            $"api/product/{id}"
        );

        return response.IsSuccessStatusCode;
    }

    // CSV UPLOAD
    public async Task<bool> UploadProductsCsvAsync(
        IFormFile file)
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

        using var content = new MultipartFormDataContent();

        await using var stream =
            file.OpenReadStream();

        using var fileContent =
            new StreamContent(stream);

        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue("text/csv");

        content.Add(
            fileContent,
            "file",
            file.FileName
        );

        var response = await client.PostAsync(
            "api/product/bulk-upload",
            content
        );

        return response.IsSuccessStatusCode;
    }
}