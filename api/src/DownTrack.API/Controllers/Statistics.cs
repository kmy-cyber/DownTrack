using DownTrack.Application.DTO.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StatisticsController : ControllerBase
{
    private readonly StatisticsServicesContainer _statisticsServices;

    public StatisticsController(StatisticsServicesContainer statisticsServices)
    {
        _statisticsServices = statisticsServices;
    }

    [HttpGet]
    [Route("Admin")]
    public async Task<ActionResult<AdminStatisticsDto>> GetStatisticsForAdmin()
    {
        var statistics = await _statisticsServices.AdminStatisticsService.GetStatisticsForAdmins();
        return Ok(statistics);
    }

    [HttpGet]
    [Route("Technician")]
    public async Task<ActionResult<TechnicianStatisticsDto>> GetStatisticsByTechnician(int technicianId)
    {
        var statistics = await _statisticsServices.TechnicianStatisticsService.GetStatisticsByTechnician(technicianId);
        return Ok(statistics);
    }

    [HttpGet]
    [Route("Receptor")]
    public async Task<ActionResult<ReceptorStatisticsDto>> GetStatisticsByReceptor(int receptorId)
    {
        var statistics = await _statisticsServices.ReceptorStatisticsService.GetStatisticsByReceptor(receptorId);
        return Ok(statistics);
    }

    [HttpGet]
    [Route("Director")]
    public async Task<ActionResult<DirectorStatisticsDto>> GetStatisticsByDirector()
    {
        var statistics = await _statisticsServices.DirectorStatisticsService.GetStatisticsByDirector();
        return Ok(statistics);
    }

    [HttpGet]
    [Route("SectionManager")]
    public async Task<ActionResult<ManagerStatisticsDto>> GetStatisticsByManager(int sectionManager)
    {
        var statistics = await _statisticsServices.SectionManagerStatisticsService
                                    .GetStatisticsBySectionManager(sectionManager);
        return Ok(statistics);
    }

}