using Shared.Enums;

namespace Shared.DTOs.User;

public record AppUserDto
{
    public string Id { get; init; }

    public string UserName { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public UserStatus Status { get; init; }
}
