


namespace DownTrack.Domain.Entities;

public class Employee : GenericEntity
{
    
    public string Name { get; set; } = null!;
    public string UserRole { get; set; }  = null!;
    public string Email {get;set;}= null!;
    public string UserName {get;set;}= null!;

    public User? User {get;set;}
    
    // SectionManager
    public ICollection<Evaluation> GivenEvaluations {get;set;}= new List<Evaluation>();
    public ICollection<Section> Sections {get;set;} = new List<Section>();

    public ICollection<TransferRequest> TransferRequests{ get; set; } = new List<TransferRequest>();

    public ICollection<Transfer> Transfers{ get; set; } = new List<Transfer>();

}