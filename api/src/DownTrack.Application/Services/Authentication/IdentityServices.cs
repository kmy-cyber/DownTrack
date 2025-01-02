
using AutoMapper;
using DownTrack.Application.Authentication;
using DownTrack.Application.Common.Authentication;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.Services.Authentication;

public class IdentityService : IIdentityService
{
    private readonly IIdentityManager _identityManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IEmployeeServices _employeeServices;
    private readonly ITechnicianServices _technicianServices;

    public IdentityService(
                IJwtTokenGenerator jwtTokenGenerator,
                IMapper mapper,
                IIdentityManager identityManager,
                IEmployeeServices employeeServices,
                ITechnicianServices technicianServices
                )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _identityManager = identityManager;
        _employeeServices = employeeServices;
        _technicianServices = technicianServices;
    }

    public async Task<bool> LoginUserAsync(LoginUserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        if (user == null)
        {
            return false;
        }

        var savedUser = await _identityManager.CheckCredentialsAsync(user.UserName!, userDto.Password);

        if (savedUser) return savedUser;

        return false;
    }

    public async Task<string> RegisterUserAsync(RegisterUserDto userDto)
    {
        try
        {
            if (!UserRoleHelper.IsValidRole(userDto.UserRole))

                throw new Exception("Invalid Role");




            var user = _mapper.Map<User>(userDto);
            var savedUser = await _identityManager.CreateUserAsync(user, userDto.Password);
            await _identityManager.AddRoles(savedUser.Id, userDto.UserRole);

            if (userDto.UserRole == UserRole.Technician.ToString())
            {
                var technician = _mapper.Map<TechnicianDto>(userDto);

                await _technicianServices.CreateAsync(technician);
            }

            else
            {
                var employee = _mapper.Map<EmployeeDto>(userDto);

                await _employeeServices.CreateAsync(employee);
            }
            
            var token = _jwtTokenGenerator.GenerateToken(savedUser);

            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw; 
        }
    }
}