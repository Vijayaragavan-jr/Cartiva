using System.ComponentModel.DataAnnotations;

namespace Cartiva.Models;

public partial class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }
}