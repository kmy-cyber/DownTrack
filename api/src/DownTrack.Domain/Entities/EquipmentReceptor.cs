

namespace DownTrack.Domain.Entities;

public class EquipmentReceptor : Employee
{
    public int DepartmentId {get;set;}
    public Department Department {get;set;} = null!;
    public ICollection<Transfer> AcceptedTransfers {get;set;} = null!;

}