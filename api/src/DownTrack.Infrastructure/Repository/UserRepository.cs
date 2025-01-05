

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

    public async Task<User> CreateAsync(User element, CancellationToken cancellationToken = default)
    {
        var result = await _userManager.CreateAsync(element);

        if(!result.Succeeded)
            throw new Exception("Error creating user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        
        return element;
    }
    public async Task<User> GetByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.
                        FirstOrDefaultAsync(u=> u.Id == elementId.ToString());
        if(user is null)
            throw new Exception($"Error searching user: {elementId}" );
        
        return user;

    }
    public async Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.
                        FirstOrDefaultAsync(u=> u.Id == elementId.ToString());
        
        if(user is null)
            throw new Exception($"Error deleting user: {elementId}" );
        
        await _userManager.DeleteAsync(user);

    }
    
}