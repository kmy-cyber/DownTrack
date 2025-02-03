

namespace DownTrack.Application.DTO.Statistics;

public class DirectorStatisticsDto
{
    public int NumberOfEquipments { get; set; }

    public int NumberOfCompletedMaintenances { get; set; }
    public int NumberOfTotalMaintenances { get; set; }

    public Dictionary<string, int> AcceptedDecommissionsByMonth { get; set; } = null!;

    public Dictionary<string, int> TransfersByMonth { get; set; } = null!;

    public Dictionary<string, double> MaintenanceCostByMonth { get; set; } = null!;


}