using System.Text.Json.Serialization;
using DownTrack.Domain.Enum;

namespace DownTrack.Domain.Entities
{
    public class Equipment : GenericEntity
    {
        public string Name { get; set; } = "name";
        public string Type { get; set; } = "type";
        public EquipmentStatus Status { get; set; }
        public DateTime DateOfadquisition { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public EquipmentDecommissioning? EquipmentDecommissioning { get; set; }
    }
}