
namespace DownTrack.Domain.Entities
{
    public class Equipment : GenericEntity
    {
        public string Name { get; set; } = "name";
        public string Type { get; set; } = "type";
        public string Status { get; set; } = "status";
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
        public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();
        public ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();

    }
}