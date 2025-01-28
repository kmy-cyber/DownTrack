namespace DownTrack.Application.DTO;

public class GetTransferRequestDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int SectionManagerId { get; set; }
    public string SectionManagerUserName {get;set;}= null!;
    public int EquipmentId { get; set; }
    public int ArrivalDepartmentId { get; set; }
    public int ArrivalSectionId {get;set;}
    public string ArrivalDepartmentName {get;set;} =null!;
    public string ArrivalSectionName {get;set;} =null!;

}

