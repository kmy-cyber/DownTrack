
namespace DownTrack.Application.DTO;

public class EquipmentReceptorDto
{
    public int Id { get; set;}
    public string Name { get; set; } = null!;
    public int DepartmentId {get;set;}
    public int SectionId {get;set;}
    public string UserRole = "EquipmentReceptor";
}
