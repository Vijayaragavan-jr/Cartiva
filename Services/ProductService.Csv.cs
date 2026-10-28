using Cartiva.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartiva.Services;

public partial class ProductService
{
    // BULK CSV PRODUCT UPLOAD
    public async Task<object> BulkUploadProducts(IFormFile file)
    {
        var successfulProducts = 0;
        var failedRows = 0;

        var errors = new List<object>();

        using var reader = new StreamReader(file.OpenReadStream());

        // Read header
        var headerLine = await reader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(headerLine))
        {
            return new
            {
                message = "CSV file is empty"
            };
        }

        var rowNumber = 1;

        while (!reader.EndOfStream)
        {
            rowNumber++;

            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
                continue;

            var columns = line.Split(',');

            if (columns.Length < 3)
            {
                failedRows++;

                errors.Add(new
                {
                    row = rowNumber,
                    message = "Invalid CSV format"
                });

                continue;
            }

            var name = columns[0].Trim();
            var description = columns[1].Trim();
            var priceText = columns[2].Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                failedRows++;

                errors.Add(new
                {
                    row = rowNumber,
                    message = "Product name is required"
                });

                continue;
            }

            if (!decimal.TryParse(priceText, out var price) || price <= 0)
            {
                failedRows++;

                errors.Add(new
                {
                    row = rowNumber,
                    message = "Price must be greater than zero"
                });

                continue;
            }

            var product = new Product
            {
                Name = name,
                Description = string.IsNullOrWhiteSpace(description)
                    ? null
                    : description,
                Price = price,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);

            successfulProducts++;
        }

        await _context.SaveChangesAsync();

        return new
        {
            message = "CSV upload completed",
            successfulProducts,
            failedRows,
            errors
        };
    }
}