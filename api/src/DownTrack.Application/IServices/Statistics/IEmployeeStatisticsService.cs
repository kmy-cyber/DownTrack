using DownTrack.Application.DTO.Statistics;

namespace DownTrack.Application.IServices.Statistics;


public interface IEmployeeStatisticsService
{
    Task<AdminStatisticsDto> GetStatisticsForAdmins();
    Task<TechnicianStatisticsDto> GetStatisticsByTechnician(int technicianId);
    Task<ReceptorStatisticsDto> GetStatisticsByReceptor(int receptorId);
    Task<DirectorStatisticsDto> GetStatisticsByDirector();
    Task<ManagerStatisticsDto> GetStatisticsBySectionManager(int managerId);

}