

using DownTrack.Domain.Enum;

namespace DownTrack.Domain.Entities;

public class Employee : GenericEntity
{
    public string Name { get; set; } = null!;
    public string Role { get; set; }  = UserRole.Technician;
}