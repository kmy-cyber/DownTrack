namespace DownTrack.Application.DTO;

public class GetTransferRequestDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = null!;
    public int? SectionManagerId { get; set; }
    public string? SectionManagerUserName { get; set; }
    public int? SourceSectionId { get; set; }
    public string? SourceSectionName { get; set; }
    public int SourceDepartmentId { get; set; }
    public string SourceDepartmentName { get; set; } = null!;
    public int ArrivalDepartmentId { get; set; }
    public int ArrivalSectionId { get; set; }
    public string ArrivalDepartmentName { get; set; } = null!;
    public string ArrivalSectionName { get; set; } = null!;
    public int EquipmentId { get; set; }
    public string EquipmentName { get; set; } = null!;
    public string EquipmentStatus { get; set; } = null!;
    public string EquipmentType { get; set; } = null!;

}

