


namespace DownTrack.Domain.Entities;

public class Employee : GenericEntity
{
    public string Name { get; set; } = null!;
    public string UserRole { get; set; } = null!;

    public ICollection<TransferRequest> TransferRequests{ get; set; } = new List<TransferRequest>();

    public ICollection<Transfer> Transfers{ get; set; } = new List<Transfer>();

}