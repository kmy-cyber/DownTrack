

namespace DownTrack.Application.DTO;

public class TechnicianDto : EmployeeDto
{
    public string Specialty { get; set; } = null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }

}