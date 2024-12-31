using System.Text.Json.Serialization;

namespace DownTrack.Domain.Entities;

public class EquipmentDecommissioning : GenericEntity
{
    public int? TechnicianId { get; set; }
    public int? EquipmentId { get; set; }
    public DateTime DateOfDecommissioning { get; set; } = DateTime.Now;
    public string? DecommissioningReason { get; set; }

    [JsonIgnore]
    public Technician? Technician { get; set; }
    [JsonIgnore]
    public Equipment? Equipment { get; set; }
}