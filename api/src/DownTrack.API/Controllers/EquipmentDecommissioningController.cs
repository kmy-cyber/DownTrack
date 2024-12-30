using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using DownTrack.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentDecommissioningController : ControllerBase
{
    private readonly IEquipmentDecommissioningServices _equipmentDecommissioningServices;


    public EquipmentDecommissioningController(IEquipmentDecommissioningServices equipmentDecommissioningServices)
    {
        _equipmentDecommissioningServices = equipmentDecommissioningServices;

    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateEquipmentDecommissioning(EquipmentDecommissioningDto equipmentDecommissioning)
    {          
        await _equipmentDecommissioningServices.CreateAsync(equipmentDecommissioning);
        return Ok("Equipment Decommissioning added successfully");
    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<EquipmentDecommissioning>>> GetAllEquipmentDecommissioning()
    {
        var results = await _equipmentDecommissioningServices.ListAsync();

        return Ok(results);
    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateEquipmentDecommissioning(EquipmentDecommissioningDto equipmentDecommissioning)
    {
        var result = await _equipmentDecommissioningServices.UpdateAsync(equipmentDecommissioning);
        return Ok(result);
    }

    [HttpDelete]
    [Route("Delete")]

    public async Task<IActionResult> DeleteEquipmentDecommissioning(int equipmentDecommissioningId)
    {
        await _equipmentDecommissioningServices.DeleteAsync(equipmentDecommissioningId);

        return Ok("Equipment Decommissioning deleted successfully");
    }
}