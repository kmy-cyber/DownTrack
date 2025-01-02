using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentServices _equipmentService;

    public EquipmentController(IEquipmentServices equipmentServices)
    {
        _equipmentService = equipmentServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateEquipment(EquipmentDto equipment)
    {
        await _equipmentService.CreateAsync(equipment);

        return Ok("Equipment added successfully");
    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<Equipment>>> GetAllEquipment()
    {
        var results = await _equipmentService.ListAsync();

        return Ok(results);

    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateEquipment(EquipmentDto equipment)
    {
        var result = await _equipmentService.UpdateAsync(equipment);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEquipment(int equipmentId)
    {
        await _equipmentService.DeleteAsync(equipmentId);

        return Ok("Equipment deleted successfully");
    }
}