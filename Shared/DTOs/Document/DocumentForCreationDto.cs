using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Document;

public record DocumentForCreationDto
{
    public IFormFile DocumentFile { get; init; } = null!;
}
