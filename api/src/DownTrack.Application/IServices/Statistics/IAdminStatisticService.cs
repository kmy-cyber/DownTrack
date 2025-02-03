
using DownTrack.Application.DTO.Statistics;

namespace DownTrack.Application.IServices.Statistics;

public interface IAdminStatisticsService
{
    Task<AdminStatisticsDto> GetStatisticsForAdmins();

}