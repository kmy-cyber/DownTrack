


namespace DownTrack.Application.DTO;

public class EquipmentReceptor
{
    public int Id { get; set;}
    public string Name { get; set; } = null!;
    public int DepartamentId {get;set;}
    public int SectionId {get;set;}
    public const string UserRole = "EquipmentReceptor";
}