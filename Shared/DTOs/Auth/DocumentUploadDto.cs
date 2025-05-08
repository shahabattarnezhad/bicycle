using Microsoft.AspNetCore.Http;

namespace Shared.DTOs.Auth;

public record DocumentUploadDto
{
    public string DocumentType { get; init; } = string.Empty;

    
}
