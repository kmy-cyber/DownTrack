
using DownTrack.Domain.Enum;

namespace DownTrack.Domain.Entities;
public class Equipment : GenericEntity
{
    public string Name { get; set; } = "name";
    public string Type { get; set; } = "type";
    public EquipmentStatus EquipmentStatus { get; set; } = EquipmentStatus.Active;
    public DateTime DateOfadquisition { get; set; } = DateTime.Now;
    public ICollection<EquipmentDecommissioning>? EquipmentDecommissionings { get; set; } = new List<EquipmentDecommissioning>();
}
