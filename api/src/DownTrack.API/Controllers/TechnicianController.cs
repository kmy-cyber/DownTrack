
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TechnicianController : ControllerBase
{
    private readonly ITechnicianServices _technicianService;

    public TechnicianController(ITechnicianServices technicianServices)
    {
        _technicianService = technicianServices;
    }


    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<Technician>>> GetAllTechnician()
    {
        var results = await _technicianService.ListAsync();

        return Ok(results);

    }


    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<Technician>> GetUserById(int technicianId)
    {
        var result = await _technicianService.GetByIdAsync(technicianId);

        if (result == null)
            return NotFound($"Technician with ID {technicianId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedUser ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _technicianService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }



}

