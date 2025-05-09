using Microsoft.AspNetCore.Http;
using Service.Contracts.Interfaces.Auth;
using System.Security.Claims;

namespace Service.Services.Auth;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            IsAuthenticated = true;
            UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            FirstName = user.FindFirstValue(ClaimTypes.GivenName);
            LastName = user.FindFirstValue(ClaimTypes.Surname);
            Email = user.FindFirstValue(ClaimTypes.Email);
            UserStatus = user.FindFirstValue("status");
        }
    }

    public string UserId { get; } = null!;
    public string? FirstName { get; }
    public string? LastName { get; }
    public string? Email { get; }
    public string? UserStatus { get; }
    public bool IsAuthenticated { get; }
}
