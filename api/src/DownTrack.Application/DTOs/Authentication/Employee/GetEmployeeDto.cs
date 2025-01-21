

namespace DownTrack.Application.Dto;

public class GetEmployeeDto
{
    public int Id {get;set;}
    public string Name { get; set; } = null!;
    public string UserRole { get; set; } =null!;
    public string UserName {get;set;} = null!;
    public string Email {get;set;}= null!;

}