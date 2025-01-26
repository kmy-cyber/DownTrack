using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentQueryServices _equipmentQueryService;
    private readonly IEquipmentCommandServices _equipmentCommandService;

    public EquipmentController(IEquipmentCommandServices equipmentCommandServices,
                                IEquipmentQueryServices equipmentQueryServices)
    {
        _equipmentQueryService = equipmentQueryServices;
        _equipmentCommandService = equipmentCommandServices;
    }

    #region Command

    [HttpPost]
    [Route("POST")]
    public async Task<IActionResult> CreateEquipment(EquipmentDto equipment)
    {
        await _equipmentCommandService.CreateAsync(equipment);

        return Ok("Equipment added successfully");
    }

      [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateEquipment(EquipmentDto equipment)
    {
        var result = await _equipmentCommandService.UpdateAsync(equipment);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{equipmentId}")]

    public async Task<IActionResult> DeleteEquipment(int equipmentId)
    {
        await _equipmentCommandService.DeleteAsync(equipmentId);

        return Ok("Equipment deleted successfully");
    }

    #endregion

    #region Query

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<GetEquipmentDto>> GetUserById(int equipmentId)
    {
        var result = await _equipmentQueryService.GetByIdAsync(equipmentId);

        if (result == null)
            return NotFound($"Equipment with ID {equipmentId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEquipment ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentQueryService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    #endregion

  
}