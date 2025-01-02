
using AutoMapper;
using DownTrack.Application.Authentication;
using DownTrack.Application.Common.Authentication;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IRepository;
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
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITechnicianRepository _technicianRepository;

    public IdentityService(
                IJwtTokenGenerator jwtTokenGenerator,
                IMapper mapper,
                IIdentityManager identityManager,
                IEmployeeRepository employeeRepository,
                ITechnicianRepository technicianRepository
                )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _identityManager = identityManager;
        _employeeRepository = employeeRepository;
        _technicianRepository = technicianRepository;
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
                Console.WriteLine(userDto.UserRole);
                var technicianDto = _mapper.Map<TechnicianDto>(userDto);
                Console.WriteLine(technicianDto.UserRole);
                var technician = _mapper.Map<Technician>(technicianDto);
                Console.WriteLine(technician.UserRole);

                await _technicianRepository.CreateAsync(technician);
            }

            else
            {
                Console.WriteLine(userDto.UserRole);
                var employeeDto = _mapper.Map<EmployeeDto>(userDto);

                Console.WriteLine(employeeDto.UserRole);
                var employee = _mapper.Map<Employee>(employeeDto);
                Console.WriteLine(employee.UserRole);
                await _employeeRepository.CreateAsync(employee);
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