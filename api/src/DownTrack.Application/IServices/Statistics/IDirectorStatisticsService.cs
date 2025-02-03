using DownTrack.Application.DTO.Statistics;

namespace DownTrack.Application.IServices.Statistics;


public interface IDirectorStatisticsService
{
    Task<DirectorStatisticsDto> GetStatisticsByDirector();
}