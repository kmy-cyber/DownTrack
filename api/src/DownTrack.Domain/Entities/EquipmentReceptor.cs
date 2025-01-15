

namespace DownTrack.Domain.Entities;

public class EquipmentReceptor : Employee
{
    public int DepartamentId {get;set;}
    public int SectionId{get;set;}
    public Department Departament {get;set;} = null!;

    //usar Section si se necesita acceder directamente en alguna consulta frecuente
    //public Section Section {get;set;} = null!;


}