using Shared.DTOs.Base;
using Shared.DTOs.User;

namespace Shared.DTOs.Document;

public record DocumentDto : BaseEntityDto<Guid>
{
    public string DocumentType { get; init; } = string.Empty;

    public string Path { get; init; } = string.Empty;

    public DateTime UploadedAt { get; init; }

    public DateTime VerifiedAt { get; init; }

    public string VerifiedById { get; init; } = default!;
    public AppUserDto? VerifiedByAppUser { get; init; }

    public string AppUserId { get; init; } = default!;
    public AppUserDto? AppUser { get; init; }
}
