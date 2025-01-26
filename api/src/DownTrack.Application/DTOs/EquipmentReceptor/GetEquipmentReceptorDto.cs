
namespace DownTrack.Application.DTO;

public class GetEquipmentReceptorDto
{
    public int Id { get; set;}
    public string Name { get; set; } = null!;
    public int DepartmentId {get;set;}
    public int SectionId {get;set;}
    public string DepartmentName {get;set;} = null!;
    public string SectionName {get;set;} =null!;
    public string UserName {get;set;} =null!;
    public string UserRole = "EquipmentReceptor";
}
