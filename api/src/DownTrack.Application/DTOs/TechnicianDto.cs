

namespace DownTrack.Application.DTO;

public class TechnicianDto : UserDto
{
    public string Specialty { get; set; } = string.Empty;
    public double Salary { get; set; }
    public int ExpYears { get; set; }

}