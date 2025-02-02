
namespace DownTrack.Domain.Entities;

public class DoneMaintenance : GenericEntity
{
    public int? TechnicianId { get; set; }
    public string Type { get; set; } = null!;
    public int? EquipmentId { get; set; }    
    public DateTime Date { get; set; } 
    public double Cost { get; set; }
    public Equipment? Equipment { get; set; }
    public Technician? Technician { get; set; }
    public bool Finish {get;set;}
    
}