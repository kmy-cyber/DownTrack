using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
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

    public async Task<ActionResult<IEnumerable<EquipmentDecommissioningDto>>> GetAllEquipmentDecommissioning()
    {
        var results = await _equipmentDecommissioningServices.ListAsync();

        return Ok(results);

    }
    
    [HttpGet]
    [Route("{equipmentDecommissioningId}/GET_BY_ID")]
    public async Task<ActionResult<EquipmentDecommissioningDto>> GetEquipmentDecommissioningById(int equipmentDecommissioningId)
    {
        var result = await _equipmentDecommissioningServices.GetByIdAsync(equipmentDecommissioningId);

        if (result == null)
            return NotFound($"Equipment Decommissioning with ID {equipmentDecommissioningId} not found");

        return Ok(result);

    }

    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateEquipmentDecommissioning(EquipmentDecommissioningDto equipmentDecommissioning)
    {
        await _equipmentDecommissioningServices.UpdateAsync(equipmentDecommissioning);

        return Ok("Equipment Decommissioning updated successfully");
    }

    [HttpDelete]
    [Route("{equipmentDecommissioningId}/delete")]
    public async Task<IActionResult> DeleteEquipmentDecommissioning(int equipmentDecommissioningId)
    {
        await _equipmentDecommissioningServices.DeleteAsync(equipmentDecommissioningId);

        return Ok("Equipment Decommissioning deleted successfully");
    }
    [HttpPost]
    [Route("{equipmentDecommissioningId}/accept")]
    public async Task<IActionResult> AcceptDecommissioning(int equipmentDecommissioningId)
    {
        await _equipmentDecommissioningServices.AcceptDecommissioningAsync(equipmentDecommissioningId);

        return Ok("Equipment Decommissioning accepted successfully");
    }



}