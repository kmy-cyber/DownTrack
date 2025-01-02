
using DownTrack.Domain.Enum;

namespace DownTrack.Domain.Entities
{
    public class Equipment : GenericEntity
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public EquipmentStatus Status { get; set; }
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;        
        public EquipmentDecommissioning EquipmentDecommissioning { get; set; } = null!;
    }
}