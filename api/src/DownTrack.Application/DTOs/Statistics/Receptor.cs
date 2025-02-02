
namespace DownTrack.Application.DTO.Statistics;

public class ReceptorStatisticsDto
{
    public int PendingTransfers { get; set; } // Total request transfers
    public int PendingDecommissions { get; set; } // Total decommissions assigned to this receptor
    public int TotalEquipments { get; set; } // Total equipments belonging to the same department and section
    public Dictionary<string, int> AcceptedDecommissionsPerMonth { get; set; } = null!; // Total decommissions accepted per month
    public Dictionary<string, int> ProcessedTransfersPerMonth { get; set; } = null!;// Total transfers accepted or registered per month
}