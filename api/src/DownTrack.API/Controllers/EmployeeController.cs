

using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeServices _employeeService;

    public EmployeeController(IEmployeeServices employeeServices)
    {
        _employeeService = employeeServices;
    }


    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllEmployee()
    {
        var results = await _employeeService.AllAsync();

        return Ok(results);

    }

    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<Employee>> GetEmployeeById(int employeeId)
    {
        var result = await _employeeService.GetByIdAsync(employeeId);

        if (result == null)
            return NotFound($"Employee with ID {employeeId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEmployee ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _employeeService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        await _employeeService.DeleteAsync(employeeId);

        return Ok("Employee deleted successfully");
    }


}

