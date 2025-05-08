using Shared.Enums;

namespace Shared.DTOs.Document;

public record DocumentForVerificationDto
{
    public Guid Id { get; init; }
    
    public string? Notes { get; init; }

    public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
}
