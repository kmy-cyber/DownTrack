

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
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RegisterUser(RegisterUserDto registerDto)
    {

        var result = await _identityService.RegisterUserAsync(registerDto);
        Console.WriteLine(result);
        return Ok(result);

    }

    [HttpPost]
    [Route("login")]

    public async Task<IActionResult> LoginUser(LoginUserDto loginDto)
    {

        var token = await _identityService.LoginUserAsync(loginDto);
<<<<<<< HEAD
        Console.WriteLine("+=================================");
        Console.WriteLine(token);
         Console.WriteLine("+=================================");
=======
        Console.WriteLine(token);
>>>>>>> 4bc28fe8d9c25724a8760837e4b863d6ad1369fe
        return Ok(token);

    }

    [HttpPut]
    [Route("PUT")]
    public async Task<IActionResult> UpdateUser(UpdateUserDto updateDto )
    {
        await _identityService.UpdateUserAsync(updateDto);

        return Ok();
    }
}