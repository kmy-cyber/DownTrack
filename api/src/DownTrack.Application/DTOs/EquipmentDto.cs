namespace DownTrack.Application.DTO;

// DTO para el registro de un Equipment
public class EquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime DateOfadquisition { get; set; } = DateTime.Now;
    public int LocationId {get;set;}
}
