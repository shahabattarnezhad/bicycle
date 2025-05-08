using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth;

public record LoginDto
{
    [Required(ErrorMessage = "User name is required")]
    public string UserName { get; init; } = string.Empty;


    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; } = string.Empty;

    public bool RememberMe { get; set; }
}
