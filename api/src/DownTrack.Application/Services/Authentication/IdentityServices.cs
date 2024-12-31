
// using AutoMapper;
// using DownTrack.Application.Common.Authentication;
// using DownTrack.Application.DTO.Authentication;
// using DownTrack.Application.IServices;
// using DownTrack.Application.IServices.Authentication;
// using DownTrack.Domain.Entities;

// namespace DownTrack.Application.Services.Authentication;

// public class IdentityService : IIdentityService
// {

//     private readonly IJwtTokenGenerator _jwtTokenGenerator;
//     private readonly IMapper _mapper;
//     private readonly IUserServices _userServices;
    
//     public IdentityService(
//                 IJwtTokenGenerator jwtTokenGenerator,
//                 IMapper mapper,
//                 IUserServices userServices
//                 )
//     {
//         _jwtTokenGenerator = jwtTokenGenerator;
//         _mapper = mapper;
//         _userServices = userServices;
//     }

//     public async Task<bool> CheckCredentialAsync(LoginUserDto userDto)
//     {
//         return true;
//     }
// }