


namespace DownTrack.Domain.Entities;

public class Employee : GenericEntity
{
    
    public string Name { get; set; } = null!;
    public string UserRole { get; set; }  = null!;

    public User? User {get;set;}
    // SectionManager
    public ICollection<Evaluation> GivenEvaluations {get;set;}= new List<Evaluation>();
    public ICollection<Section> Sections {get;set;} = new List<Section>();

}