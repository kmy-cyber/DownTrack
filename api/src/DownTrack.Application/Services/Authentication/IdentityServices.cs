
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
        Console.WriteLine(userDto.DepartmentId);
        try
        {
            if (!UserRoleHelper.IsValidRole(userDto.UserRole))

                throw new Exception("Invalid Role");

            var user = _mapper.Map<User>(userDto);

            if (userDto.UserRole == UserRole.ShippingSupervisor.ToString())
            {
                var supervisor = _mapper.Map<Employee>(userDto);

                await _unitOfWork.GetRepository<Employee>().CreateAsync(supervisor);

                await _unitOfWork.CompleteAsync();

                return "Not token for this user";

            }

            else if (userDto.UserRole == UserRole.Technician.ToString())
            {

                var technician = _mapper.Map<Technician>(userDto);

                technician.User = user;
                Console.WriteLine("======================================");
                Console.WriteLine(technician.User);
                Console.WriteLine("======================================");
                await _unitOfWork.GetRepository<Technician>().CreateAsync(technician);


            }

            else if (userDto.UserRole == UserRole.EquipmentReceptor.ToString())
            {

                Console.WriteLine(userDto.DepartmentId);
                var equipmentReceptor = _mapper.Map<EquipmentReceptor>(userDto);
                equipmentReceptor.User = user;
                Console.WriteLine(equipmentReceptor.User);

                Console.WriteLine(equipmentReceptor.DepartmentId);
                var department = await _unitOfWork.GetRepository<Department>()
                                          .GetByIdAsync(equipmentReceptor.DepartmentId);

                Console.WriteLine($"======================={department.Id}========={department.SectionId}=======");

                if (userDto.SectionId != department.SectionId)
                    throw new Exception($"Department with Id :{department.Id} not belong to Section with Id :{userDto.SectionId}");

                equipmentReceptor.Department = department;

                await _unitOfWork.GetRepository<EquipmentReceptor>().CreateAsync(equipmentReceptor);


            }

            else
            {

                var employee = _mapper.Map<Employee>(userDto);
                employee.User = user;
                Console.WriteLine(employee.User);
                await _unitOfWork.GetRepository<Employee>().CreateAsync(employee);



            }


            var savedUser = await _identityManager.CreateUserAsync(user, userDto.Password);

            await _identityManager.AddRoles(savedUser.Id.ToString(), userDto.UserRole);

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


    public async Task UpdateUserAsync(UpdateUserDto updateDto)
    {
        try
        {
            

            if (updateDto.UserRole == UserRole.Technician.ToString())
            {
                var technician = _mapper.Map<Technician>(updateDto);

                _unitOfWork.GetRepository<Technician>().Update(technician);

                await _unitOfWork.CompleteAsync();
                
                
            }

            else if (updateDto.UserRole == UserRole.EquipmentReceptor.ToString())
            {
                var receptor = _mapper.Map<EquipmentReceptor>(updateDto);

                _unitOfWork.GetRepository<EquipmentReceptor>().Update(receptor);
            }

            else
            {
                var employee = _mapper.Map<Employee>(updateDto);

                _unitOfWork.GetRepository<Employee>().Update(employee);
            }

            if(updateDto.UserRole != "ShippingSupervisor")
                    await _unitOfWork.UserRepository.UpdateByIdAsync(updateDto.Id, updateDto.Email);
            
            await _unitOfWork.CompleteAsync();
            
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


}