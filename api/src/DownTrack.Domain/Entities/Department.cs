
namespace DownTrack.Domain.Entities;

public class Department : GenericEntity
{
    public string Name { get; set; } = null!;
    public int SectionId {get; set;}

    // todo dpto tiene obligado una Seccion
    public Section Section { get; set; } = null!;
    public ICollection<EquipmentReceptor> EquipmentReceptors {get;set;} = new List<EquipmentReceptor>();

}