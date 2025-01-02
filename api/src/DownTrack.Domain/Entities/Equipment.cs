
namespace DownTrack.Domain.Entities
{
    public class Equipment : GenericEntity
    {
        public string Name { get; set; } = "name";
        public string Type { get; set; } = "type";
        public string Status { get; set; } = "status";
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
    }
}