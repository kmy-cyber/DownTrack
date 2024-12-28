


using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRespository;
using AutoMapper;
using DownTrack.Domain.Enitites;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to agency and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class UserServices : IUserServices
{

    private readonly ITechnicianRepository _technicianRepository;
    private readonly IMapper _mapper;

    public UserServices(IUserRepository UserRepository, IMapper mapper)
    {
        _UserRepository = UserRepository;
        _mapper = mapper;
    }



    /// <summary>
    /// Creates a new user based on the provided UserDto.
    /// </summary>
    /// <param name="userDto">The DTO containing user details to be created.</param>
    /// <returns>A Task representing the asynchronous operation, with an userDto as the result.</returns>
    public async Task<UserDto> CreateAsync(UserDto userDto)
    {
        // map the DTOs (UserDto) to a domain entity (User) 
        var result = _mapper.Map<User>(userDto);

        // method of the repository is called to insert the User entity into the database
        await _userRepository.CreateAsync(result);

        // map the new created user entity to a UserDTO
        return _mapper.Map<UserDto>(result);
    }



    /// <summary>
    /// Updates an existing user's information.
    /// </summary>
    /// <param name="userDto">The DTO containing updated user details.</param>
    /// <returns>A Task representing the asynchronous operation, with an UserDto as the result.</returns>
    public async Task<UserDto> UpdateAsync(UserDto userDto)
    {
        var result = _userRepository.GetById(userDto.Id);

        // Maps the provided UserDto to the existing User entity, updates the User in the database, 
        _mapper.Map(technicianDto, result);

        await _technicianRepository.UpdateAsync(result);

        /// and returns the updated technician as a TechnicianDto.
        return _mapper.Map<TechnicianDto>(result);
    }



    /// <summary>
    /// Retrieves a list of all technicians.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of TechnicianDto as the result.</returns>
    public async Task<IEnumerable<TechnicianDto>> ListAsync()
    {
        //fetches all technicians from the repository
        var results = await _technicianRepository.ListAsync();

        //return them as a enumerable of TechnicanDto objects
        return results.Select(_mapper.Map<TechnicianDto>);

    }



    /// <summary>
    /// Deletes a technician by its ID.
    /// </summary>
    /// <param name="technicianDto">The technician's ID to be deleted.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(int technicianDto)
    {
        await _technicianRepository.DeleteByIdAsync(technicianDto);
    }



}