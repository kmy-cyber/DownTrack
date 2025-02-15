using System.ComponentModel.DataAnnotations;

namespace DownTrack.Application.DTO.Authentication;

public class RegisterUserDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "User role is required")]
    public string UserRole { get; set; } = null!;

    public string Specialty { get; set; } = null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }
    public int DepartmentId { get; set; }
    public int SectionId { get; set; }
}
