
namespace DownTrack.Domain.Entities;
public class Equipment : GenericEntity
{

    public class Equipment : GenericEntity
    {
        public string Name { get; set; } = "name";
        public string Type { get; set; } = "type";
        public string Status { get; set; } = "status";
        public int LocationId {get; set;}
        public Department Location {get; set;} = new Department();

        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
        public ICollection<DoneMaintenance> DoneMaintenances { get; set; } = new List<DoneMaintenance>();
        
        public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();
        public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();

}

