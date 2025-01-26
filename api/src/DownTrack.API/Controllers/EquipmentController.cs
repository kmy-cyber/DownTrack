using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        // Obtener el claim "role"
        // var roleClaim = User?.FindFirst(ClaimTypes.Role);  // ClaimTypes.Role es el nombre estándar para el claim de rol

        // if(roleClaim == null)
        // {
        //     Console.WriteLine("es null");
        //     throw new Exception();
        // }    

        // Console.WriteLine(roleClaim.Value);

        // if (roleClaim == null || roleClaim.Value != "Technician")
        // {
        //     return Unauthorized();  // Si el claim "role" no es igual a "Technician", se deniega el acceso
        // }
        await _equipmentService.CreateAsync(equipment);

        return Ok("Equipment added successfully");
    }


    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<EquipmentDto>> GetUserById(int equipmentId)
    {
        var result = await _equipmentService.GetByIdAsync(equipmentId);

        if (result == null)
            return NotFound($"Equipment with ID {equipmentId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEquipment([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentService.GetPagedResultAsync(paged);

        return Ok(result);

    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateEquipment(EquipmentDto equipment)
    {
        var result = await _equipmentService.UpdateAsync(equipment);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{equipmentId}")]

    public async Task<IActionResult> DeleteEquipment(int equipmentId)
    {
        await _equipmentService.DeleteAsync(equipmentId);

        return Ok("Equipment deleted successfully");
    }






    //endpoint for the section manager
    [HttpGet("equipments/section-manager/{sectionManagerId}")]
    public async Task<IActionResult> GetPagedEquipmentsBySectionManagerId(
    int sectionManagerId,
    [FromQuery] PagedRequestDto paged)
    {
        // Asignar la URL base para construir los enlaces de paginación
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        // Llamar al servicio para obtener los resultados paginados
        var result = await _equipmentService.GetPagedEquipmentsBySectionManagerIdAsync(sectionManagerId, paged);

        // Retornar la respuesta
        return Ok(result);
    }

}