using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts.Interfaces.Auth;
using Service.Services.Constants;
using Shared.DTOs.Auth;
using Shared.DTOs.User;
using Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services.Auth;

internal sealed class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    private AppUser? _user;

    public AuthService(
        IMapper mapper,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
        RoleManager<AppRole> roleManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
    }

    public async Task<string> CreateTokenAsync(AppUser user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim("status", user.Status.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            expires: DateTime.UtcNow.AddHours(6),
            claims: authClaims,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ApiResponse<AppUserDto>> RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto == null)
            throw new EntityIsNullException("The regidter is null");

        var appUser = _mapper.Map<AppUser>(registerDto);

        var result = await _userManager.CreateAsync(appUser, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<AppUserDto>("User registration failed", errors);
        }

        var roleExists = await _roleManager.RoleExistsAsync(RoleConstants.Customer);
        if (!roleExists)
        {
            return new ApiResponse<AppUserDto>("Role 'Customer' does not exist");
        }

        var roleResult = await _userManager.AddToRoleAsync(appUser, RoleConstants.Customer);
        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            return new ApiResponse<AppUserDto>("Failed to assign role", errors);
        }

        var userDto = _mapper.Map<AppUserDto>(appUser);
        return new ApiResponse<AppUserDto>(userDto, "User registered successfully");
    }

    public async Task<ApiResponse<AppUserDto>> LoginAsync(LoginDto loginDto)
    {
        _user = await _userManager.FindByNameAsync(loginDto.UserName!);

        if (_user is null)
        {
            return new ApiResponse<AppUserDto>("Username or password is incorrect.");
        }

        //var result = await _signInManager.PasswordSignInAsync(
        //            _user!.UserName!,
        //            loginDto.Password!,
        //            loginDto.RememberMe,
        //            lockoutOnFailure: true);

        var result =
            await _signInManager.CheckPasswordSignInAsync(_user!, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return new ApiResponse<AppUserDto>("Username or password is incorrect.");
        }

        //if (user.Status != UserStatus.Approved)
        //    throw new NoAccessException("Your account is not approved yet.");

        var userDto = _mapper.Map<AppUserDto>(_user);
        return new ApiResponse<AppUserDto>(userDto, "User logged in successfully");
    }
}
