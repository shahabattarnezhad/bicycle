using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace Shared.DTOs.Auth;

public record RegisterDto
{
    [Required]
    public string UserName { get; init; } = string.Empty;

    [Required]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}