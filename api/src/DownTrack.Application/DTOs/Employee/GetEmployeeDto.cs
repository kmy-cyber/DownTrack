

namespace DownTrack.Application.DTO;

//DTO de mostrado de caracteristicas de un empleado

public class GetEmployeeDto : EmployeeDto
{
    public string? Email {get;set;}
    public string? UserName {get;set;}
    
}