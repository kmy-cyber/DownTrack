namespace DownTrack.Application.DTO;

public class GetTransferRequestDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public int SectionManagerId { get; set; }
    public string SectionManagerUserName { get; set; } = null!;
    public int EquipmentId { get; set; }
    public string? EquipmentName { get; set; }
    public string? EquipmentStatus { get; set; }
    public string? EquipmentType { get; set; }
    public string? RequestDepartmentId { get; set; }
    public string? RequestDepartmentName { get; set; }
    public string? RequestSectionId { get; set; }
    public string? RequestSectionName { get; set; }
    public int ArrivalDepartmentId { get; set; }
    public int ArrivalSectionId { get; set; }
    public string ArrivalDepartmentName { get; set; } = null!;
    public string ArrivalSectionName { get; set; } = null!;

}

