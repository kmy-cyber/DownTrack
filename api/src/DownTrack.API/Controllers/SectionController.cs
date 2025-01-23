
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ISectionServices _sectionService;

    public SectionController(ISectionServices sectionServices)
    {
        _sectionService = sectionServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateSection(SectionDto section)
    {
        Console.WriteLine(section.Name);
        await _sectionService.CreateAsync(section);

        return Ok("Section added successfully");
    }


    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<Section>> GetUserById(int sectionId)
    {
        var result = await _sectionService.GetByIdAsync(sectionId);

        if (result == null)
            return NotFound($"Section with ID {sectionId} not found");

        return Ok(result);

    }


    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedSection ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _sectionService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }


    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateSection(SectionDto section)
    {
        var result = await _sectionService.UpdateAsync(section);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteSection(int sectionId)
    {
        await _sectionService.DeleteAsync(sectionId);

        return Ok("Section deleted successfully");
    }
}

