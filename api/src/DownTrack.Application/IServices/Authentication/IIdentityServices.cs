

using DownTrack.Application.DTO.Authentication;

namespace DownTrack.Application.IServices.Authentication;

public interface IIdentityService
{
    Task<string> RegisterUserAsync (RegisterUserDto userDto);
    Task <bool> LoginUserAsync(LoginUserDto userDto);
}