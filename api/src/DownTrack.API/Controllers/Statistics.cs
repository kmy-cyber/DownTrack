using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StatisticsController : ControllerBase
{
    private readonly IEmployeeStatisticsService _employeeStatisticsService;

    public StatisticsController(IEmployeeStatisticsService employeeStatisticsService)

    {
        _employeeStatisticsService = employeeStatisticsService;
    }

    [HttpGet]
    [Route("Admin")]
    public async Task<ActionResult<AdminStatisticsDto>> GetStatisticsForAdmin()
    {
        var statistics = await _employeeStatisticsService.GetStatisticsForAdmins();

        return Ok(statistics);
    }

    [HttpGet]
    [Route("Technician")]
    public async Task<ActionResult<TechnicianStatisticsDto>> GetStatisticsByTechnician(int technicianId)
    {
        var statistics = await _employeeStatisticsService.GetStatisticsByTechnician(technicianId);

        return Ok(statistics);
    }

    [HttpGet]
    [Route("Receptor")]
    public async Task<ActionResult<ReceptorStatisticsDto>> GetStatisticsByReceptor(int receptorId)
    {
        var statistics = await _employeeStatisticsService.GetStatisticsByReceptor(receptorId);

        return Ok(statistics);
    }

    [HttpGet]
    [Route("Director")]
    public async Task<ActionResult<DirectorStatisticsDto>> GetStatisticsByDirector()
    {
        var statistics = await _employeeStatisticsService.GetStatisticsByDirector();

        return Ok(statistics);
    }

    [HttpGet]
    [Route("SectionManager")]
    public async Task<ActionResult<DirectorStatisticsDto>> GetStatisticsByManager(int sectionManager)
    {
        var statistics = await _employeeStatisticsService.GetStatisticsBySectionManager(sectionManager);

        return Ok(statistics);
    }

}