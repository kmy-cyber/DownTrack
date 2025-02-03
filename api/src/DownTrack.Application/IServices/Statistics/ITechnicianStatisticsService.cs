using DownTrack.Application.DTO.Statistics;

namespace DownTrack.Application.IServices.Statistics;

public interface ITechnicianStatisticsService
{
    Task<TechnicianStatisticsDto> GetStatisticsByTechnician(int technicianId);
}
