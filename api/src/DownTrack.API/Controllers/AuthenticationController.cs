using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthenticationController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Route("register")]
    // [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RegisterUser(RegisterUserDto registerDto)
    {
        Console.WriteLine(registerDto.DepartmentId);
        var result = await _identityService.RegisterUserAsync(registerDto);
        Console.WriteLine(result);
        return Ok(result);

    }

    [HttpPost]
    [Route("login")]

    public async Task<IActionResult> LoginUser(LoginUserDto loginDto)
    {

        var token = await _identityService.LoginUserAsync(loginDto);
        return Ok(token);

    }

    [HttpPut]
    [Route("PUT")]
    public async Task<IActionResult> UpdateUser(UpdateUserDto updateDto )
    {
        System.Console.WriteLine("Aaaa==we=fger=gekrfogjdfioghuidsofhgushfjsdbhjfbsdjhkfbiuwehfuiweshbfjkbwsdhjfkbsdiufbsidhbfhsaAAAAAAA=============");
        System.Console.WriteLine($"{updateDto.Id.ToString()},{updateDto.Name},{updateDto.Email},{updateDto.DepartmentId},{updateDto.ExpYears},{updateDto.Salary},{updateDto.Specialty},{updateDto.UserName},{updateDto.UserRole}");
        await _identityService.UpdateUserAsync(updateDto);
        

        return Ok();
    }
}