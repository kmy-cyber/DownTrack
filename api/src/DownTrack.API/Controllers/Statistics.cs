using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StatisticsController : ControllerBase
{
    private readonly IEmployeeQueryServices _employeeQueryService;

    public StatisticsController(IEmployeeQueryServices employeeQueryServices)

    {
        _employeeQueryService = employeeQueryServices;
    }

    [HttpGet]
    [Route("Admin")]
    public async Task<ActionResult<AdminStatisticsDto>> GetStatisticsForAdmin()
    {
        var statistics = await _employeeQueryService.GetStatisticsForAdmins();

        return Ok(statistics);
    }

    [HttpGet]
    [Route("Technician")]
    public async Task<ActionResult<TechnicianStatisticsDto>> GetStatisticsByTechnician(int technicianId)
    {
        var statistics = await _employeeQueryService.GetStatisticsByTechnician(technicianId);

        return Ok(statistics);
    }

    [HttpGet]
    [Route("Receptor")]
    public async Task<ActionResult<ReceptorStatisticsDto>> GetStatisticsByReceptor(int receptorId)
    {
        var statistics = await _employeeQueryService.GetStatisticsByReceptor(receptorId);

        return Ok(statistics);
    }

}