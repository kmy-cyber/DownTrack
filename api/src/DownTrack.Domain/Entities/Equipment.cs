
namespace DownTrack.Domain.Entities;
public class Equipment : GenericEntity
{

        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
        public ICollection<DoneMaintenance> DoneMaintenances { get; set; } = new List<DoneMaintenance>();
        
        public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();

}

