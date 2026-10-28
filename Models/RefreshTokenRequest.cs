using System.ComponentModel.DataAnnotations;

namespace Cartiva.Models;

public partial class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}