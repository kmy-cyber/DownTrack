

namespace DownTrack.Domain.Entities;

public class Evaluation : GenericEntity
{
    public int TechnicianId {get;set;} // not null
    public Technician Technician {get;set;} = null!; 
    public int? SectionManagerId {get;set;} // can be null
    public Employee? SectionManager {get;set;}
    public string Description {get;set;} = "notEvaluation";
    
}