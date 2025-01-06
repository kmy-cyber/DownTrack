


namespace DownTrack.Domain.Entities;

public class Employee : GenericEntity
{
    public string Name { get; set; } = null!;
    public string UserRole { get; set; }  = null!;

    // SectionManager
    public ICollection<Evaluation> GivenEvaluations {get;set;}= new List<Evaluation>();

}