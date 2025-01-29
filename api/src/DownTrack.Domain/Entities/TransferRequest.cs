
namespace DownTrack.Domain.Entities;

public class TransferRequest : GenericEntity
{
    public int? SectionManagerId { get; set; }

    public string Status {get; set;} = "Unregistered";
    public  Employee? SectionManager { get; set; } 

    public int EquipmentId { get; set; }
    public  Equipment Equipment { get; set; } = null!;

    public DateTime Date { get; set; } 

    public  int ArrivalDepartmentId { get; set; } 
    public  Department ArrivalDepartment { get; set; } =null!;

    public int? TransferId {get;set;}
    public Transfer? Transfer {get;set;}

}
