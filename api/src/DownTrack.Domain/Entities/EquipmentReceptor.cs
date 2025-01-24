

namespace DownTrack.Domain.Entities;

public class EquipmentReceptor : Employee
{
    public int DepartamentId {get;set;}
    public Department Departament {get;set;} = null!;
    public ICollection<Transfer> AcceptedTransfers {get;set;} = null!;

}