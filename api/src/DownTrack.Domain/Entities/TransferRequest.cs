using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DownTrack.Domain.Entities;

public class TransferRequest : GenericEntity
{
    public int? EmployeeId { get; set; }

    public  Employee? Employee { get; set; } 

    public int? EquipmentId { get; set; }
    public  Equipment? Equipment { get; set; }

    public DateTime Date { get; set; } // Fecha de la solicitud de transferencia

    public  int? DepartmentId { get; set; }
    public  int? SectionId { get; set; }

    public  Department? Department { get; set; }

}
