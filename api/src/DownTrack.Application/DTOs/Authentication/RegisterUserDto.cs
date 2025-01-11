

namespace DownTrack.Application.DTO.Authentication;

public class RegisterUserDto
{
    public int Id {get;set;}
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserRole { get; set; } = null!;
    public string Specialty { get; set; } = null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }
    public int DepartamentId {get;set;}
    public int SectionId {get;set;}
}