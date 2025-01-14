namespace DownTrack.Application.DTO;

public class DoneMaintenanceDto
{
    public int Id { get; set; }
    public int? TechnicianId { get; set; }
    public string Type { get; set; } = "type";
    public int? EquipmentId { get; set; }    
    public DateTime Date { get; set; } = DateTime.Now;
    public double Cost { get; set; } = 0.0;
}