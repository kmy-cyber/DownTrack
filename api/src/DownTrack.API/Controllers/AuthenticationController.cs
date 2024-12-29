


using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController: ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthenticationController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Route("login")]

    public async Task<IActionResult> Login (LoginUserDto loginDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        //check credentials

        var token = await _identityService.CheckCredentialAsync(loginDto);

        if(!token)
            return Unauthorized(new { Message = "Wrong Credentials" });
        
        return Ok(new {Token = token});

    }

}   