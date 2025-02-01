namespace DownTrack.Domain.Entities;

public class Department : GenericEntity
{
    public string Name { get; set; } = null!; 
    public int SectionId {get; set;}

    // todo dpto tiene obligado una Seccion
    public Section Section { get; set; } = null!;
    public ICollection<EquipmentReceptor> EquipmentReceptors {get;set;} = new List<EquipmentReceptor>();
    public ICollection<TransferRequest> OutgoingTransferRequests { get; set; } = new List<TransferRequest>();

    public ICollection<TransferRequest> IncomingTransferRequests {get;set;} = new List<TransferRequest>();
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();

}