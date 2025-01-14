

namespace DownTrack.Domain.Entities;

public class Evaluation : GenericEntity
{
    public int TechnicianId {get;set;}
    public Technician Technician {get;set;} = null!; // confia que nunca sera null
    public int SectionManagerId {get;set;}
    public Employee SectionManager {get;set;}= null!; // confia que nunca sera null
    public string Description {get;set;} = "notEvaluation";
    
}