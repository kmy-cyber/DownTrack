
namespace DownTrack.Domain.Entities;

public class EquipmentDecommissioning : GenericEntity
{
    public int? TechnicianId { get; set; }
    public int? EquipmentId { get; set; }
    public DateTime DateOfDecommissioning { get; set; } = DateTime.Now;
    public string DecommissioningReason { get; set; } = null!;

    public Technician Technician { get; set; } = null!;
    public Equipment Equipment { get; set; } = null!;
}