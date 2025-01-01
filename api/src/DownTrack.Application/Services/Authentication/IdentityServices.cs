
using AutoMapper;
using DownTrack.Application.Authentication;
using DownTrack.Application.Common.Authentication;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.Services.Authentication;

public class IdentityService : IIdentityService
{
    private readonly IIdentityManager _identityManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IEmployeeServices _employeeServices;

    private readonly ITechnicianServices _technicianServices;

    // falta tmb los servicios para definir las tablas donde insertar
    // de momento no se inserta en la tabla de Empleados

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
            var user = _mapper.Map<User>(userDto);
            var savedUser = await _identityManager.CreateUserAsync(user, userDto.Password);
            await _identityManager.AddRoles(savedUser.Id, userDto.UserRole);

            if(userDto.UserRole == UserRole.Technician)
            {
                var technician = _mapper.Map<TechnicianDto>(userDto);
                
                await _technicianServices.CreateAsync(technician);
            }

            else
            {
                var employee = _mapper.Map<EmployeeDto>(userDto);
                
                await _employeeServices.CreateAsync(employee);
            }
            // aqui se define saber a que tabla de empleado agregar usnado su servicio
            var token = _jwtTokenGenerator.GenerateToken(savedUser);

            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw; // O maneja el error adecuadamente en tu aplicación
        }
    }
}