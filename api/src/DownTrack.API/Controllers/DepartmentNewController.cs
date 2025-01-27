using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentQueryServices _departmentQueryService;
    private readonly IDepartmentCommandServices _departmentCommandService;

    public DepartmentController(IDepartmentQueryServices departmentQueryServices,
                                IDepartmentCommandServices departmentCommandServices)
    {
        _departmentQueryService = departmentQueryServices;
        _departmentCommandService = departmentCommandServices;
    }

    #region Command
    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateDepartmen(DepartmentDto department)
    {
        
        await _departmentCommandService.CreateAsync(department);


        return Ok("Department added successfully");
    }


      [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateDepartment(DepartmentDto department)
    {
        var result = await _departmentCommandService.UpdateAsync(department);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteDepartment(int departmentId)
    {
        await _departmentCommandService.DeleteAsync(departmentId);

        return Ok("Department deleted successfully");
    }

    #endregion

    #region Query

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<IEnumerable<GetDepartmentDto>>> GetDepartmentById(int departmentId)
    {
        var result = await _departmentQueryService.GetByIdAsync(departmentId);

        if (result == null)
            return NotFound($"Department with ID {departmentId} not found");

        return Ok(result);

    }


    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedDepartment ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _departmentQueryService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<GetDepartmentDto>>> GetAll()
    {
        var result = await _departmentQueryService.ListAsync();

        return Ok(result);

    }

    [HttpGet]
    [Route("GetAllDepartment_In_Section")]
    public async Task<ActionResult<IEnumerable<GetDepartmentDto>>> GetAllDepartmentInSection (int sectionId)
    {
        var result = await _departmentQueryService.GetAllDepartmentsInSection(sectionId);

        return Ok(result);
    }

    #endregion
  
}

