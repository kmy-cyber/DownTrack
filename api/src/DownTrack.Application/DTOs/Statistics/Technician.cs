
namespace DownTrack.Application.DTO.Statistics;

public class TechnicianStatisticsDto
{
    public int Id {get;set;}
    public int Maintenances {get;set;} // ya ha hecho
    public int MaintenancesInProgress {get;set;} // esta realizando
    public int Decomissions {get;set;}
    public Dictionary<string,int> MaintenanceByMonth {get;set;}= null!;
    public Dictionary<string,int> DecomissionsByMonth {get;set;} = null!;
    public Dictionary<string,int> EquipmentByStatus {get;set;} = null!;
    
}