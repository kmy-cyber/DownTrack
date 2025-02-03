
namespace DownTrack.Application.DTO.Statistics;

public class PerformanceTechnicianDto
{
    /// <summary>
    /// Dictionary containing the number of evaluations received, categorized by type.
    /// Key: Evaluation Type (e.g., "Good", "Regular", "Bad")
    /// Value: Number of evaluations of that type
    /// </summary>
    public Dictionary<string, int> EvaluationsByType { get; set; } = new();

    /// <summary>
    /// Total number of maintenance tasks completed by the technician.
    /// </summary>
    public int CompletedMaintenances { get; set; }

    /// <summary>
    /// Total number of decommissioning requests proposed by the technician.
    /// </summary>
    public int ProposedDecommissions { get; set; }
}
