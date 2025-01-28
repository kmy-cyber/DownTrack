namespace DownTrack.Application.DTO;

public class GetDoneMaintenanceDto
{
    public int Id { get; set; }
    public int? TechnicianId { get; set; }
    public string? TechnicianUserName {get;set;}
    public string Type { get; set; } = string.Empty;
    public int? EquipmentId { get; set; }   
    public string? EquipmentName {get;set;} 
    public DateTime Date { get; set; } = DateTime.Now;
    public double Cost { get; set; } 
}