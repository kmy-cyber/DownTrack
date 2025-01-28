using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentReceptorController : ControllerBase
{
    private readonly IEquipmentReceptorQueryServices _equipmentReceptorQueryService;

    public EquipmentReceptorController(IEquipmentReceptorQueryServices equipmentReceptorQueryServices)
    {
        _equipmentReceptorQueryService = equipmentReceptorQueryServices;
    }



    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<GetEquipmentReceptorDto>> GetUserById(int equipmentReceptorId)
    {
        var result = await _equipmentReceptorQueryService.GetByIdAsync(equipmentReceptorId);

        if (result == null)
            return NotFound($"EquipmentReceptor with ID {equipmentReceptorId} not found");

        return Ok(result);

    }


    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEquipmentReceptor ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentReceptorQueryService.GetAllPagedResultAsync(paged);
        
        return Ok (result);
        
    }
    
    [HttpGet]
    [Route("GetAll")]

    public async Task<IActionResult> GetAllEquipmentReceptor ()
    {

        var result = await _equipmentReceptorQueryService.ListAsync();
        
        return Ok (result);
        
    }


    

}

