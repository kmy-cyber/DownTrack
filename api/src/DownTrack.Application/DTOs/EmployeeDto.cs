

using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public UserRole role { get; set; } = null!;

}