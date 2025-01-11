
namespace DownTrack.Domain.Entities;

public class Department : GenericEntity
{
    public string Name { get; set; } = null!;

    public int SectionId {get; set;}
    public Section Section { get; set; } = null!;
}