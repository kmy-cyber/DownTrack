using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentServices _departmentService;

    public DepartmentController(IDepartmentServices departmentServices)
    {
        _departmentService = departmentServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateDepartmen(DepartmentDto department)
    {
        
        await _departmentService.CreateAsync(department);


        return Ok("Department added successfully");
    }


    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
    {
        var results = await _departmentService.ListAsync();

        return Ok(results);

    }

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<Department>> GetDepartmentById(int departmentId)
    {
        var result = await _departmentService.GetByIdAsync(departmentId);

        if (result == null)
            return NotFound($"Department with ID {departmentId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedDepartment ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _departmentService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateDepartment(DepartmentDto department)
    {
        var result = await _departmentService.UpdateAsync(department);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteDepartment(int departmentId)
    {
        await _departmentService.DeleteAsync(departmentId);

        return Ok("Department deleted successfully");
    }
}

