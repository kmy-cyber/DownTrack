namespace DownTrack.Domain.Entities;

public class EquipmentDecommissioning : GenericEntity
{
    public int TechnicianId { get; set; }
    public int EquipmentId { get; set; }
    public DateTime DateOfDecommissioning { get; set; } = DateTime.Now;
    public string? DecommissioningReason { get; set; }

    public Technician? Technician { get; set; }
    public Equipment? Equipment { get; set; }
}