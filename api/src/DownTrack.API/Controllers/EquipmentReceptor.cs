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
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<EquipmentReceptor>>> GetAllEquipmentReceptor()
    {
        var results = await _equipmentReceptorService.ListAsync();

        return Ok(results);

    }


    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<EquipmentReceptor>> GetUserById(int equipmentReceptorId)
    {
        var result = await _equipmentReceptorService.GetByIdAsync(equipmentReceptorId);

        if (result == null)
            return NotFound($"EquipmentReceptor with ID {equipmentReceptorId} not found");

        return Ok(result);

    }


}

