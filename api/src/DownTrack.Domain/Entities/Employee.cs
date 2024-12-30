

using DownTrack.Domain.Enum;

namespace DownTrack.Domain.Entities;

public class Employee : GenericEntity
{
    public string Name { get; set; } = string.Empty;
    public UserRole role { get; set; }
}