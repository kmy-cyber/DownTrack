using System.Text.Json.Serialization;
using DownTrack.Domain.Enum;

namespace DownTrack.Domain.Entities;

public class EquipmentDecommissioning : GenericEntity
{
    public int? TechnicianId { get; set; }
    public int? EquipmentId { get; set; }
    public int? ReceptorId { get; set; }
    public DateTime Date { get; set; }
    public string Cause { get; set; } = string.Empty;
    public DecommissioningStatus Status { get; set; } = DecommissioningStatus.pending;


    [JsonIgnore]
    public Technician? Technician { get; set; }
    [JsonIgnore]
    public Equipment? Equipment { get; set; }
    [JsonIgnore]
    public EquipmentReceptor? Receptor { get; set; }
    

}