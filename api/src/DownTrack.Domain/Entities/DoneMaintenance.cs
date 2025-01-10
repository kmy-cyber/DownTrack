using System.Text.Json.Serialization;

namespace DownTrack.Domain.Entities;

public class DoneMaintenance : GenericEntity
{
    public int? TechnicianId { get; set; }
    public string Type { get; set; } = "type";
    public int? EquipmentId { get; set; }    
    public DateTime Date { get; set; } = DateTime.Now;
    public double Cost { get; set; } = 0.0;

    [JsonIgnore]
    public Equipment? Equipment { get; set; }
    [JsonIgnore]
    public Technician? Technician { get; set; }
}