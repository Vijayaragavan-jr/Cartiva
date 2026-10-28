namespace Cartiva.Web.Models;

public class DashboardViewModel
{
    public int TotalUsers { get; set; }

    public int TotalProducts { get; set; }

    public int TotalCarts { get; set; }

    public int TotalCartItems { get; set; }

    public decimal CartTotal { get; set; }
}