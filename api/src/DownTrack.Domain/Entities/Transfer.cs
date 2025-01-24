
namespace DownTrack.Domain.Entities;

public class Transfer : GenericEntity
{
    public int RequestId { get; set; }
    public TransferRequest TransferRequest{get; set;} = null!;

    public int? ShippingSupervisorId {get; set;}
    public Employee? ShippingSupervisor {get; set;}
    public int? EquipmentReceptorId {get; set;}
    public EquipmentReceptor? EquipmentReceptor{get; set;}
    public DateTime Date { get; set; } 
}
