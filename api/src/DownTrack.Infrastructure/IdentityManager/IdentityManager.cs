using DownTrack.Application.Authentication;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure;

public class IdentityManager : IIdentityManager
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityManager(UserManager<User> userManage, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManage;
        _roleManager = roleManager;
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Error creating user: {errors}");
        }

        return user;
    }


    public async Task AddRoles(string userId, string role)
    {
        
        var existingRole = await _roleManager.FindByNameAsync(role);
        if (existingRole == null)
        {
            var newRole = new IdentityRole(role);
            var roleResult = await _roleManager.CreateAsync(newRole);
            if (!roleResult.Succeeded)
            {
                throw new Exception($"Error creating role  {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }

        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"User with ID {userId} not found");
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);

        if (!addRoleResult.Succeeded)
        {
            throw new Exception($"Error adding role {role} to user : {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
        }
    }

    public async Task<User?> CheckCredentialsAsync(string username, string password)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.UserName!.Equals(username));

        if (user is null)
            return null;

        var valid = await _userManager.CheckPasswordAsync(user, password);

        if(!valid)
            return null;
            
        return user;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }


}
