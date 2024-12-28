
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRespository;
using AutoMapper;
using DownTrack.Domain.Enitites;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to user and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class UserServices : IUserServices
{

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserServices(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }



    /// <summary>
    /// Creates a new user based on the provided UserDto.
    /// </summary>
    /// <param name="userDto">The DTO containing user details to be created.</param>
    /// <returns>A Task representing the asynchronous operation, with an UserDto as the result.</returns>
    public async Task<UserDto> CreateAsync(UserDto userDto)
    {
        // map the DTOs (userDto) to a domain entity (User) 
        var result = _mapper.Map<User>(userDto);

        // method of the repository is called to insert the User entity into the database
        await _userRepository.CreateAsync(result);

        // map the new created User entity to a UserDTO
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

        // Maps the provided UserDto to the existing user entity, updates the user in the database, 
        _mapper.Map(userDto, result);

        await _userRepository.UpdateAsync(result);

        /// and returns the updated user as a UserDto.
        return _mapper.Map<UserDto>(result);
    }



    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of UserDto as the result.</returns>
    public async Task<IEnumerable<UserDto>> ListAsync()
    {
        //fetches all users from the repository
        var results = await _userRepository.ListAsync();

        //return them as a enumerable of TechnicanDto objects
        return results.Select(_mapper.Map<UserDto>);

    }



    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="userDto">The user's ID to be deleted.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    public async Task DeleteAsync(int userDto)
    {
        await _userRepository.DeleteByIdAsync(userDto);
    }



    /// <summary>
    /// Retrieves a user by their ID
    /// </summary>
    /// <param name="userDto">The user's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the user</returns>
    public async Task<UserDto> GetByIdAsync(int userDto)
    {
        var result = await _userRepository.GetByIdAsync(userDto);

        /// and returns the updated user as a UserDto.
        return _mapper.Map<UserDto>(result);

    }



}