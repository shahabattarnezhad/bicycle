using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Service.Contracts.Interfaces.Auth;
using Service.Contracts.Interfaces.Helpers;
using Shared.DTOs.Auth;
using Shared.Enums;
using Shared.Responses;

namespace Presentation.Controllers;

[Route("api/auth")]
public class AuthController : ApiControllerBase
{
    private readonly IServiceManager _service;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IFileService _fileService;

    public AuthController(UserManager<AppUser> userManager,
                          SignInManager<AppUser> signInManager,
                          ITokenService tokenService,
                          IServiceManager service,
                          IFileService fileService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _service = service;
        _fileService = fileService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var result =
            await _service.AuthService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return StatusCode(201, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var loginResponse = await _service.AuthService.LoginAsync(loginDto);

        if (!loginResponse.Success)
        {
            return BadRequest(loginResponse);
        }

        var user = new AppUser
        {
            Id = loginResponse.Data.Id,
            UserName = loginResponse.Data.UserName,
            FirstName = loginResponse.Data.FirstName,
            LastName = loginResponse.Data.LastName,
            Status = loginResponse.Data.Status
        };

        var token = await _service.AuthService.CreateTokenAsync(user);

        return Success(token, "Login successful");
    }
}
