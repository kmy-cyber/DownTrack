namespace DownTrack.Domain.Entities;

public class Section : GenericEntity
{
    public required string Name { get; set; } = "name";

    public List<Department> Departments {get; set;}
}