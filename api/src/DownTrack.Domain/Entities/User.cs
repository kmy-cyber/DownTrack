
using Microsoft.AspNetCore.Identity;

namespace DownTrack.Domain.Entities;

public class User : IdentityUser
{
    public string Name {get;set;} = null!;

    public int IdEmployee {get;set;}
    
}