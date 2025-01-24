
namespace DownTrack.Domain.Entities;

public class TransferRequest : GenericEntity
{
    public int? EmployeeId { get; set; }

    public  Employee? SectionManager { get; set; } 

    public int EquipmentId { get; set; }
    public  Equipment Equipment { get; set; } = null!;

    public DateTime Date { get; set; } // Fecha de la solicitud de transferencia

    public  int ArrivalDepartmentId { get; set; } // departmamento al cual llegara
    public  Department ArrivalDepartment { get; set; } =null!;

    public int? TransferId {get;set;}
    public Transfer? Transfer {get;set;}

}
