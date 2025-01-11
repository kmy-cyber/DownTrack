

using DownTrack.Domain.Entities;

namespace DownTrack.Application.Authentication;


public interface IIdentityManager
{
    Task<User> CreateUserAsync(User user, string password);
    Task AddRoles(string userId, string role);
    Task<bool> CheckCredentialsAsync(string username, string password);
    Task<bool> IsInRoleAsync(string userId, string role);
}