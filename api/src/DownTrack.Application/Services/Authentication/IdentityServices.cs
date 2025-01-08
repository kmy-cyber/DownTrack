
using AutoMapper;
using DownTrack.Application.Authentication;
using DownTrack.Application.Common.Authentication;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.Services.Authentication;

public class IdentityService : IIdentityService
{
    private readonly IIdentityManager _identityManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public IdentityService(
                IJwtTokenGenerator jwtTokenGenerator,
                IMapper mapper,
                IIdentityManager identityManager,
                IUnitOfWork unitOfWork
                )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _mapper = mapper;
        _identityManager = identityManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> LoginUserAsync(LoginUserDto userDto)
    {
        // Map the login DTO to the User model.
        var user = _mapper.Map<User>(userDto);

        // Check if the mapping or the user object is null.
        if (user == null) 
            throw new Exception();

        // Validate the user's credentials.
        var savedUser = await _identityManager.CheckCredentialsAsync(user.UserName!, userDto.Password);

        // If the credentials are invalid, return null.
        if (savedUser is null)
            throw new Exception();
        
        // If the credentials are valid, generate a token for the authenticated user.
        return await _jwtTokenGenerator.GenerateToken(savedUser);

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
                Console.WriteLine("================================================================================================");
                var technicianDto = _mapper.Map<TechnicianDto>(userDto);
                
                var technician = _mapper.Map<Technician>(technicianDto);
                
                Console.WriteLine($"Technician: {technician.Name}, {technician.UserRole}");  


                await _unitOfWork.GetRepository<Technician>().CreateAsync(technician);

                await _unitOfWork.CompleteAsync();
            }

            else
            {
                Console.WriteLine("================================================================================================");
                Console.WriteLine(userDto.UserRole);
                var employeeDto = _mapper.Map<EmployeeDto>(userDto);

                Console.WriteLine(employeeDto.UserRole);
                var employee = _mapper.Map<Employee>(employeeDto);
                Console.WriteLine(employee.UserRole);

                Console.WriteLine("================================================================================================");
                await _unitOfWork.GetRepository<Employee>().CreateAsync(employee);

                await _unitOfWork.CompleteAsync();
            }
            
            var token = await _jwtTokenGenerator.GenerateToken(savedUser);

            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw; 
        }
    }
}