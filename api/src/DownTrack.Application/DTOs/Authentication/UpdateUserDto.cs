
namespace DownTrack.Application.DTO.Authentication;

public class UpdateUserDto
{
    public int Id {get;set;}
    public string UserRole { get; set; } = null!;
    public string Name {get;set;} = null!;
    public string Email {get;set;} = null!;
    public string Password {get;set;}= null!;
    public int Salary { get; set; }
    public string Specialty { get; set; } = null!;
    public int ExpYears { get; set; }
    public int DepartmentId {get;set;} 
}