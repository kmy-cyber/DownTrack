

namespace DownTrack.Application.DTO;

public class GetTechnicianDto
{
    public int Id { get; set; }
    public string Specialty { get; set; } = null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }
    public string Name { get; set; } = null!;
    public string UserName {get;set;} = null!;
    public string UserRole = "Technician";
    
}