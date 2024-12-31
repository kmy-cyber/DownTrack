namespace DownTrack.Domain.Entities;

public class Department : GenericEntity
{
    public required string Name { get; set; } = "name";

    public int SectionId {get; set;}

    public required Section Section{get; set;}
}