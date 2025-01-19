

using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;


public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;


    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.
                        FirstOrDefaultAsync(u => u.Id == elementId.ToString());
        if (user is null)
            throw new Exception($"Error searching user: {elementId}");

        return user;

    }
    public async Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.
                        FirstOrDefaultAsync(u => u.Id == elementId.ToString());

        if (user is null)
            throw new Exception($"Error deleting user: {elementId}");

        await _userManager.DeleteAsync(user);

    }

    public async Task UpdateByIdAsync(int elementId, string password, string email, CancellationToken cancellationToken = default)
    {
        User user = await GetByIdAsync(elementId, cancellationToken);

        //update email
        if (!string.IsNullOrEmpty(email) && email != user.Email)
        {
            user.Email = email;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception("Error updating user email: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        //update password
        if (!string.IsNullOrEmpty(password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
                throw new Exception("Error updating user password: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }


    }

}