

using DownTrack.Application.DTO.Authentication;

namespace DownTrack.Application.IServices.Authentication;

public interface IIdentityService
{
    //Task<string> CreateUserAsync (RegisterUserDto userDto);
    Task <bool> CheckCredentialAsync(LoginUserDto userDto);
}