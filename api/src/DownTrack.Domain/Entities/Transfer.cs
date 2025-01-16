using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DownTrack.Domain.Entities;

public class Transfer : GenericEntity
{
    public int? RequestId { get; set; }
    public TransferRequest? TransferRequest{get; set;}

    public int ShippingSupervisorId {get; set;}
    public Employee? ShippingSupervisor {get; set;}
    public int EquipmentReceptorId {get; set;}
    public Employee? EquipmentReceptor{get; set;}
    public DateTime Date { get; set; } 
}
