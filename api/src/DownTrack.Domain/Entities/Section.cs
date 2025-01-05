

namespace DownTrack.Domain.Entities;

public class Section : GenericEntity
{
    public string Name { get; set; } = "name";
    public ICollection<Department> Departments{get;set;} = new List<Department>();

}