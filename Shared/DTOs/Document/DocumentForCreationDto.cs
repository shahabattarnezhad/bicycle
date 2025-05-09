using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Document;

public record DocumentForCreationDto
{
    [Required]
    public IFormFile DocumentFile { get; init; } = null!;
}
