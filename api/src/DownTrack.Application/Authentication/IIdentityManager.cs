
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Authentication;


public interface IIdentityManager
{
    /// <summary>
    /// Create a new user in the system with the specified password
    /// </summary>
    /// <param name="user">The user object to create</param>
    /// <param name="password">The password for the user</param>
    /// <returns>The created user object</returns>
    Task<User> CreateUserAsync(User user, string password);


    /// <summary>
    /// Verifies if the provided username and password are valid.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>
    /// The user object if the credentials are valid; otherwise, null.
    /// </returns>
    Task<User?> CheckCredentialsAsync(string username, string password);


    /// <summary>
    /// Checks whether a user belongs to a specific role.
    /// </summary>
    /// <param name="userId">The ID of the user to check.</param>
    /// <param name="role">The role name to verify.</param>
    /// <returns>
    /// True if the user is in the specified role; otherwise, false.
    /// </returns>
    Task<bool> IsInRoleAsync(string userId, string role);


    /// <summary>
    /// Assign a role to a user. If the role does not exist, it creates the role
    /// </summary>
    /// <param name="userId">The ID for the user to assign to role to</param>
    /// <param name="role">The name of the role to assign</param>
    /// <returns></returns>
    Task AddRoles(string userId, string role);

}