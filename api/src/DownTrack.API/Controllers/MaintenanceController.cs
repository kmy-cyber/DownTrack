using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceServices _maintenanceService;

    public MaintenanceController(IMaintenanceServices maintenanceServices)
    {
        _maintenanceService = maintenanceServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateMaintenance(MaintenanceDto maintenance)
    {
        await _maintenanceService.CreateAsync(maintenance);

        return Ok("Maintenance added successfully");
    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<Maintenance>>> GetAllMaintenance()
    {
        var results = await _maintenanceService.ListAsync();

        return Ok(results);

    }

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<Maintenance>> GetUserById(int maintenanceId)
    {
        var result = await _maintenanceService.GetByIdAsync(maintenanceId);

        if (result == null)
            return NotFound($"Maintenance with ID {maintenanceId} not found");

        return Ok(result);

    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateMaintenance(MaintenanceDto maintenance)
    {
        var result = await _maintenanceService.UpdateAsync(maintenance);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteMaintenance(int maintenanceId)
    {
        await _maintenanceService.DeleteAsync(maintenanceId);

        return Ok("Maintenance deleted successfully");
    }
}