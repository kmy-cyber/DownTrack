

namespace DownTrack.Domain.Entities;

public class EquipmentReceptor : Employee
{
    public int DepartamentId {get;set;}

    public Department Departament {get;set;} = null!;

    public int SectionId{get;set;}

    public Section section {get;set;} =null!;


    // public ICollection<Traslados> TrasladosRecibidos {get;set;}
}