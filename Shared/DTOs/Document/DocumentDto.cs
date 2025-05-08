using Shared.DTOs.Base;
using Shared.DTOs.User;
using Shared.Enums;

namespace Shared.DTOs.Document;

public record DocumentDto : BaseEntityDto<Guid>
{
    public string Path { get; init; } = string.Empty;

    public DateTime UploadedAt { get; init; }

    public DateTime VerifiedAt { get; init; }

    public string VerifiedById { get; init; } = default!;
    public AppUserDto? VerifiedByAppUser { get; init; }

    public string AppUserId { get; init; } = default!;
    public AppUserDto? AppUser { get; init; }

    public DocumentStatus Status { get; set; } = DocumentStatus.Pending;

    public string? Notes { get; init; }
}
