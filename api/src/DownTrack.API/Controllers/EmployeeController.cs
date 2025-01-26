

using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Roles;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;
[ApiController]
[Route("api/[controller]s")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeServices _employeeService;

    public EmployeeController(IEmployeeServices employeeServices)
    {
        _employeeService = employeeServices;
    }


    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int employeeId)
    {
        var result = await _employeeService.GetByIdAsync(employeeId);

        if (result == null)
            return NotFound($"Employee with ID {employeeId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllEmployee ()
    {
        var result = await _employeeService.AllAsync();

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

    [HttpGet]
    [Route("GetAllSectionManager")]

    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllSectionManager ()
    {
        var sectionManager = await _employeeService.ListAllByRole(UserRole.SectionManager);

        return Ok(sectionManager);
    }

    [HttpGet]
    [Route("GetAllShippingSupervisor")]

    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllShippingSupervisor()
    {
        var supervisor = await _employeeService.ListAllByRole(UserRole.ShippingSupervisor);

        return Ok(supervisor);
    }


    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        await _employeeService.DeleteAsync(employeeId);

        return Ok("Employee deleted successfully");
    }


}

