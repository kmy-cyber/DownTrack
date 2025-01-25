
namespace DownTrack.Domain.Entities;

public class EquipmentDecommissioning : GenericEntity
{
    public int? TechnicianId { get; set; }
    public int? EquipmentId { get; set; }
    public int? ReceptorId { get; set; }
    public DateTime Date { get; set; }
    public string Cause { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Technician? Technician { get; set; }
    public Equipment? Equipment { get; set; }
    public EquipmentReceptor? Receptor { get; set; }
    

}