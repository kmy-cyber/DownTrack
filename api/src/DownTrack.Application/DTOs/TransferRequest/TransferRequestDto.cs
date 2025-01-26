namespace DownTrack.Application.DTO;

public class TransferRequestDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int SectionManagerId { get; set; }
    public int EquipmentId { get; set; }
    public int ArrivalDepartmentId { get; set; }
    public int ArrivalSectionId {get;set;}

}

