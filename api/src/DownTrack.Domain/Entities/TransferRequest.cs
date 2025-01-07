using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DownTrack.Domain.Entities;

public class TranferRequest : GenericEntity
{
    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }

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
