

namespace DownTrack.Application.DTO.Statistics;

public class ManagerStatisticsDto
{
    // Número de transferencias solicitadas y concretadas
    public int TransferRequestsMade { get; set; }
    public int TransferRequestsCompleted { get; set; }
    
    // Número de departamentos en su sección
    public int NumberOfDepartmentsInSection { get; set; }

    public Dictionary<string,int> EvaluationsByType {get;set;} = null!;

    public Dictionary<string,int> EquipmentsByStatus {get;set;} = null!;

}