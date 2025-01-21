

namespace DownTrack.Application.DTO;

public class GetEmployeeDto
{
    public int Id {get;set;}
    public string Name {get;set;}= null!;
    public string? Email {get;set;}
    public string? UserName {get;set;}
    public string UserRole {get;set;}= null!;
}