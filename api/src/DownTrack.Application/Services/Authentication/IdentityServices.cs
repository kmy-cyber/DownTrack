
using AutoMapper;
using DownTrack.Application.Authentication;
using DownTrack.Application.Common.Authentication;
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

            if (userDto.UserRole == UserRole.Technician.ToString())
            {

                var technician = _mapper.Map<Technician>(userDto);

                await _unitOfWork.GetRepository<Technician>().CreateAsync(technician);

            }

            else if (userDto.UserRole == UserRole.EquipmentReceptor.ToString())
            {

                var equipmentReceptor = _mapper.Map<EquipmentReceptor>(userDto);

                await _unitOfWork.GetRepository<EquipmentReceptor>().CreateAsync(equipmentReceptor);

            }

            else
            {

                var employee = _mapper.Map<Employee>(userDto);

                await _unitOfWork.GetRepository<Employee>().CreateAsync(employee);


            }


            var savedUser = await _identityManager.CreateUserAsync(user, userDto.Password);

            await _identityManager.AddRoles(savedUser.Id, userDto.UserRole);

            await _unitOfWork.CompleteAsync();

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