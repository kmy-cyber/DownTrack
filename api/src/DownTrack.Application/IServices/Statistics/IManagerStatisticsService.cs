using DownTrack.Application.DTO.Statistics;

namespace DownTrack.Application.IServices.Statistics;

public interface ISectionManagerStatisticsService
{
    Task<ManagerStatisticsDto> GetStatisticsBySectionManager(int sectionManager);
}