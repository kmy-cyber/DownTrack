

using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO;

public class TechnicianDto
{
    public int Id { get; set; }
    public string Specialty { get; set; } = null!;
    public double Salary { get; set; }
    public int ExpYears { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = UserRole.Technician;

}