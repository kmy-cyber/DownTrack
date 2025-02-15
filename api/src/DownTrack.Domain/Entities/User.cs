
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DownTrack.Domain.Entities;

public class User : IdentityUser<int>
{
    
    // [Required(ErrorMessage = "Email is required")]
    // [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    // public override string? Email { get; set; }

}

public class Role : IdentityRole<int>
{
    
}
