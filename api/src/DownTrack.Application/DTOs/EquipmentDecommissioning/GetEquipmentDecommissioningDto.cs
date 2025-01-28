
namespace DownTrack.Application.DTO;

public class GetEquipmentDecommissioningDto
{
    public int Id { get; set; }
    public int? TechnicianId { get; set; }
    public string? TechnicianUserName {get;set;}
    public int? EquipmentId { get; set; }
    public int? ReceptorId { get; set; }
    public string? ReceptorUserName {get;set;}
    public DateTime Date { get; set; }
    public string Cause { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}