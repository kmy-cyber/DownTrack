
using AutoMapper;
using DownTrack.Application.Authentication;
using DownTrack.Application.Common.Authentication;
using DownTrack.Application.DTO.Authentication;
using DownTrack.Application.IServices.Authentication;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.Services.Authentication;

/// <summary>
/// Manages user authentication and registration operations.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly IIdentityManager _identityManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityService"/> class.
    /// </summary>
    /// <param name="jwtTokenGenerator">The JWT token generator.</param>
    /// <param name="mapper">The AutoMapper mapper.</param>
    /// <param name="identityManager">The identity manager.</param>
    /// <param name="unitOfWork">The unit of work.</param>

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
    /// <summary>
    /// Authenticates a user based on the provided credentials.
    /// </summary>
    /// <param name="userDto">The login DTO containing username and password.</param>
    /// <returns>A JWT token if authentication is successful, otherwise null.</returns>
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
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="userDto">The register DTO containing user details.</param>
    /// <returns>A JWT token upon successful registration.</returns>

    public async Task<string> RegisterUserAsync(RegisterUserDto userDto)
    {
        Console.WriteLine(userDto.DepartmentId);// Debug logging
        try
        {
            // Check if the user role is valid
            if (!UserRoleHelper.IsValidRole(userDto.UserRole))

                throw new Exception("Invalid Role");// Throw exception if role is invalid

            // Map the user DTO to the User model
            var user = _mapper.Map<User>(userDto);

            // Handle different user roles
            // if (userDto.UserRole == UserRole.ShippingSupervisor.ToString())
            // {
            //     // Create Shipping Supervisor entity
            //     var supervisor = _mapper.Map<Employee>(userDto);

            //     await _unitOfWork.GetRepository<Employee>().CreateAsync(supervisor);

            //     await _unitOfWork.CompleteAsync(); // Save changes to database

            //     return "Not token for this user"; // Return placeholder response


            // }

            if (userDto.UserRole == UserRole.Technician.ToString())
            {
                // Create Technician entity
                var technician = _mapper.Map<Technician>(userDto);

                technician.User = user;
                Console.WriteLine("======================================");
                Console.WriteLine(technician.User);
                Console.WriteLine("======================================");
                await _unitOfWork.GetRepository<Technician>().CreateAsync(technician);


            }

            else if (userDto.UserRole == UserRole.EquipmentReceptor.ToString())
            {
                // Get Department by ID
                Console.WriteLine(userDto.DepartmentId);
                var equipmentReceptor = _mapper.Map<EquipmentReceptor>(userDto);
                equipmentReceptor.User = user;
                Console.WriteLine(equipmentReceptor.User);

                Console.WriteLine(equipmentReceptor.DepartmentId);
                var department = await _unitOfWork.GetRepository<Department>()
                                          .GetByIdAsync(equipmentReceptor.DepartmentId);

                Console.WriteLine($"======================={department.Id}========={department.SectionId}=======");

                // Validate department section
                if (userDto.SectionId != department.SectionId)
                    throw new Exception($"Department with Id :{department.Id} not belong to Section with Id :{userDto.SectionId}");

                equipmentReceptor.Department = department;

                await _unitOfWork.GetRepository<EquipmentReceptor>().CreateAsync(equipmentReceptor);


            }

            else
            {
                
                // Create Employee entity
                var employee = _mapper.Map<Employee>(userDto);

                if(employee.UserRole == "ShippingSupervisor")
                    userDto.Password = "Password_123!";
                    
                employee.User = user;
                Console.WriteLine(employee.User);
                await _unitOfWork.GetRepository<Employee>().CreateAsync(employee);



            }

            // Create user in database
            var savedUser = await _identityManager.CreateUserAsync(user, userDto.Password);

            // Add user roles to database
            await _identityManager.AddRoles(savedUser.Id.ToString(), userDto.UserRole);

            // Save changes to database
            await _unitOfWork.CompleteAsync();
            // Generate JWT token for the new user
            var token = await _jwtTokenGenerator.GenerateToken(savedUser);

            return token;
        }
        catch (Exception ex)
        {
            // Log any errors
            Console.WriteLine($"Error: {ex.Message}");
            throw; // Re-throw the exception
        }
    }


    /// <summary>
    /// Updates an existing user's details.
    /// </summary>
    /// <param name="updateDto">The update DTO containing user details.</param>

    public async Task UpdateUserAsync(UpdateUserDto updateDto)
    {
        try
        {

            // Handle different user roles
            if (updateDto.UserRole == UserRole.Technician.ToString())
            {
                // Update Technician entity
                var technician = _mapper.Map<Technician>(updateDto);

                _unitOfWork.GetRepository<Technician>().Update(technician);

                await _unitOfWork.CompleteAsync();


            }

            else if (updateDto.UserRole == UserRole.EquipmentReceptor.ToString())
            {
                // Update EquipmentReceptor entity
                var receptor = _mapper.Map<EquipmentReceptor>(updateDto);
                Console.WriteLine(receptor.Id);
                _unitOfWork.GetRepository<EquipmentReceptor>().Update(receptor);
            }

            else
            {
                // Update Employee entity
                var employee = _mapper.Map<Employee>(updateDto);

                _unitOfWork.GetRepository<Employee>().Update(employee);
            }

            // Update user email if not ShippingSupervisor
            if (updateDto.UserRole != "ShippingSupervisor")
                await _unitOfWork.UserRepository.UpdateByIdAsync(updateDto.Id, updateDto.Email);
            // Save changes to database
            await _unitOfWork.CompleteAsync();

        }
        catch (Exception ex)
        {
            // Log any errors
            throw new Exception(ex.Message);
        }
    }


}