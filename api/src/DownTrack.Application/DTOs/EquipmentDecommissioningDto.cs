using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO;

public class EquipmentDecommissioningDto
{
    public int Id { get; set; }
    public int? TechnicianId { get; set; }
    public int? EquipmentId { get; set; }
    public int? ReceptorId { get; set; }
    public DateTime Date { get; set; }
    public string Cause { get; set; } = string.Empty;
    public DecommissioningStatus Status { get; set; } = DecommissioningStatus.pending; 
}