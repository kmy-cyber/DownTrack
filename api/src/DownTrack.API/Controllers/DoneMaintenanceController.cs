using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class DoneMaintenanceController : ControllerBase
{
    private readonly IDoneMaintenanceServices _doneMaintenanceService;

    public DoneMaintenanceController(IDoneMaintenanceServices doneMaintenanceServices)
    {
        _doneMaintenanceService = doneMaintenanceServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateDoneMaintenance(DoneMaintenanceDto doneMaintenance)
    {
        await _doneMaintenanceService.CreateAsync(doneMaintenance);

        return Ok("Done Maintenance added successfully");
    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<DoneMaintenance>>> GetAllDoneMaintenance()
    {
        var results = await _doneMaintenanceService.ListAsync();

        return Ok(results);

    }

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<DoneMaintenance>> GetDoneMaintenanceById(int doneMaintenanceId)
    {
        var result = await _doneMaintenanceService.GetByIdAsync(doneMaintenanceId);

        if (result == null)
            return NotFound($"Done Maintenance with ID {doneMaintenanceId} not found");

        return Ok(result);

    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateDoneMaintenance(DoneMaintenanceDto doneMaintenance)
    {
        var result = await _doneMaintenanceService.UpdateAsync(doneMaintenance);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteDoneMaintenance(int doneMaintenanceId)
    {
        await _doneMaintenanceService.DeleteAsync(doneMaintenanceId);

        return Ok("Done Maintenance deleted successfully");
    }
}