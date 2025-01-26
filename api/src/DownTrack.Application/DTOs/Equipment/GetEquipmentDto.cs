
namespace DownTrack.Application.DTO;

public class GetEquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime DateOfadquisition { get; set; } = DateTime.Now;
    public int DepartmentId { get; set; }
    public int SectionId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public string SectionName { get; set; } = null!;
}

