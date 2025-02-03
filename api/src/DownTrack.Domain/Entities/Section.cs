

namespace DownTrack.Domain.Entities;

public class Section : GenericEntity
{

    public string Name { get; set; }= null!;
    public int SectionManagerId {get;set;}
    public DateTime CreatedDate {get;set;}
    public Employee SectionManager {get;set;} = null!;
    public ICollection<Department> Departments{get;set;} = new List<Department>();

}