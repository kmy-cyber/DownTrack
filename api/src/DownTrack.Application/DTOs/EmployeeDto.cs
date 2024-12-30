

using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public UserRole EmployeeRole { get; set; }

}