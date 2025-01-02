

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
            throw new Exception($"Error al crear el usuario: {errors}");
        }

        return user;
    }


    public async Task AddRoles(string userId, string role)
    {
        //Verifica si el rol ya existe, si no, lo crea
        var existingRole = await _roleManager.FindByNameAsync(role);
        if (existingRole == null)
        {
            var newRole = new IdentityRole(role);
            var roleResult = await _roleManager.CreateAsync(newRole);
            if (!roleResult.Succeeded)
            {
                throw new Exception($"Error al crear el rol {role}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }

        //Asignar el rol al usuario
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new Exception($"Usuario con ID {userId} no encontrado.");
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);
        if (!addRoleResult.Succeeded)
        {
            throw new Exception($"Error al agregar rol {role} al usuario: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
        }
    }

    public async Task<bool> CheckCredentialsAsync(string username, string password)
    {
        var user = await _userManager.Users
                       .FirstOrDefaultAsync(u => u.UserName!.Equals(username));

        if (user is null)
        {
            return false;
        }

        var valid = await _userManager.CheckPasswordAsync(user, password);

        return valid;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }


}
