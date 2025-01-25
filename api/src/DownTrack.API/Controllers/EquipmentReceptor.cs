using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentReceptorController : ControllerBase
{
    private readonly IEquipmentReceptorServices _equipmentReceptorService;

    public EquipmentReceptorController(IEquipmentReceptorServices equipmentReceptorServices)
    {
        _equipmentReceptorService = equipmentReceptorServices;
    }



    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<EquipmentReceptorDto>> GetUserById(int equipmentReceptorId)
    {
        var result = await _equipmentReceptorService.GetByIdAsync(equipmentReceptorId);

        if (result == null)
            return NotFound($"EquipmentReceptor with ID {equipmentReceptorId} not found");

        return Ok(result);

    }


    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEquipmentReceptor ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentReceptorService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

}

