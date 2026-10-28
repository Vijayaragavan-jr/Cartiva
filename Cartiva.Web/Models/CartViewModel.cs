namespace Cartiva.Web.Models;

public class CartViewModel
{
    public int CartId { get; set; }

    public List<CartItemViewModel> Items { get; set; } = new();

    public decimal GrandTotal { get; set; }
}

public class CartItemViewModel
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal Total { get; set; }
}