
namespace DownTrack.Application.DTO;

public class GetTransferDto
{
    public int Id {get;set;}
    public int RequestId { get; set; }
    public int? ShippingSupervisorId { get; set; }
    public string? ShippingSupervisorName {get;set;}
    public int? EquipmentReceptorId { get; set; }
    public string? EquipmentReceptorUserName {get;set;}
    public DateTime Date { get; set; }

}

