
using DownTrack.Application.DTO.Statistics;

namespace DownTrack.Application.IServices.Statistics;


public interface IReceptorStatisticsService
{
    Task<ReceptorStatisticsDto> GetStatisticsByReceptor(int receptorId);
}