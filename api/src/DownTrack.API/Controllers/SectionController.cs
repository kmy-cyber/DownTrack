using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SectionController : ControllerBase
{
    private readonly ISectionQueryServices _sectionQueryService;
    private readonly ISectionCommandServices _sectionCommandService;


    public SectionController(ISectionQueryServices sectionQueryServices,
                             ISectionCommandServices sectionCommandServices)
    {
        _sectionQueryService = sectionQueryServices;
        _sectionCommandService = sectionCommandServices;
    }


    #region Command
    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateSection(SectionDto section)
    {
        await _sectionCommandService.CreateAsync(section);

        return Ok("Section added successfully");
    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateSection(SectionDto section)
    {
        var result = await _sectionCommandService.UpdateAsync(section);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteSection(int sectionId)
    {
        await _sectionCommandService.DeleteAsync(sectionId);

        return Ok("Section deleted successfully");
    }

    #endregion

    #region Query

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<GetSectionDto>> GetUserById(int sectionId)
    {
        var result = await _sectionQueryService.GetByIdAsync(sectionId);

        if (result == null)
            return NotFound($"Section with ID {sectionId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetBySectionName")]
    public async Task<ActionResult<GetEmployeeDto>> GetSectionByName(string sectionName)
    {
        var result = await _sectionQueryService.GetSectionByNameAsync(sectionName);

        if (result == null)
            return NotFound($"Employee with ID {sectionName} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<GetSectionDto>> GetAll()
    {
        var result = await _sectionQueryService.ListAsync();

        return Ok(result);

    }

    [HttpPost]
    [Route("GetPaged")]

    public async Task<ActionResult> GetPagedSection(PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _sectionQueryService.GetAllPagedResultAsync(paged);

        return Ok(result);

    }

    [HttpGet]
    [Route("GetSectionsByManager")]

    public async Task<ActionResult> GetSectionsByManager([FromQuery] PagedRequestDto paged, int sectionManagerId)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _sectionQueryService.GetSectionsByManagerAsync(paged, sectionManagerId);

        return Ok(result);

    }

  


    #endregion


}

