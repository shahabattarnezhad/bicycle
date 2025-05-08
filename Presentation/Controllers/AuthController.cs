using System.Net;
using Azure.Core;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Controllers.Base;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;
using Service.Contracts.Interfaces.Helpers;
using Shared.DTOs.Auth;
using Shared.DTOs.Document;
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
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var result =
            await _service.AuthService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return StatusCode(201, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return InvalidModelResponse();

        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user == null)
            return Unauthorized("Invalid username or password.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid username or password.");

        if (user.Status != UserStatus.Approved)
            throw new NoAccessException("Your account is not approved yet.");

        var token = await _service.AuthService.CreateTokenAsync(user);

        return Success(token, "ورود با موفقیت انجام شد");
    }
}
