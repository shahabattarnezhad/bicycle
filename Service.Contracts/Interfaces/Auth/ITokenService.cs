using Entities.Models;
using Shared.DTOs.User;

namespace Service.Contracts.Interfaces.Auth;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
}
