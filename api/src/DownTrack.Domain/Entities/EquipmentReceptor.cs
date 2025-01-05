

namespace DownTrack.Domain.Entities;

public class EquipmentReceptor : Employee
{
    public int DepartamentId {get;set;}

    //public Departament Departament {get;set;}

    public int SectionId{get;set;}

    public Section section {get;set;} =null!;


    // public ICollection<Traslados> TrasladosRecibidos {get;set;}
}