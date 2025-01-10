using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DownTrack.Domain.Entities;

public class TransferRequest : GenericEntity
{
    [Required]
    public int EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public required Employee Employee { get; set; }

    [Required]
    public int EquipmentId { get; set; }
    [ForeignKey(nameof(EquipmentId))]
    public required Equipment Equipment { get; set; }

    [Required]
    public DateTime Date { get; set; } // Fecha de la solicitud de transferencia

    [Required]
    public required int  DepartmentId {get; set;}
    public required int SectionId{get;set;}

    [ForeignKey(nameof(DepartmentId) + "," + nameof(SectionId))]
    public required Department Department{get; set;}

}
