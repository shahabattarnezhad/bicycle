namespace Service.Contracts.Interfaces.Auth;

public interface IUserContextService
{
    string UserId { get; }

    string? FirstName { get; }

    string? LastName { get; }

    string? Email { get; }

    string? UserStatus { get; }

    bool IsAuthenticated { get; }
}
