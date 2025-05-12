using Entities.Models;
using Shared.DTOs.Auth;
using Shared.DTOs.User;
using Shared.Responses;

namespace Service.Contracts.Interfaces.Auth;

public interface IAuthService
{
    Task<string> CreateTokenAsync(AppUser user);

    Task<ApiResponse<AppUserDto>> RegisterAsync(RegisterDto registerDto);

    Task<ApiResponse<AppUserDto>> LoginAsync(LoginDto loginDto);
}
