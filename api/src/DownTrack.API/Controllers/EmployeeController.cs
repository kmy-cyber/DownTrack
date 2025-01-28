using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Roles;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class EmployeeController : ControllerBase
{
     private readonly IEmployeeQueryServices _employeeQueryService;
     private readonly IEmployeeCommandServices _employeeCommandService;

    public EmployeeController(IEmployeeQueryServices employeeQueryServices,
                                 IEmployeeCommandServices employeeCommandServices)

    {
        _employeeQueryService = employeeQueryServices;
        _employeeCommandService = employeeCommandServices;
    }

    #region  Query
    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<GetEmployeeDto>> GetEmployeeById(int employeeId)
    {
        var result = await _employeeQueryService.GetByIdAsync(employeeId);

        if (result == null)
            return NotFound($"Employee with ID {employeeId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllEmployee ()
    {
        var result = await _employeeQueryService.ListAsync();

        return Ok(result);
    }   

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEmployee ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _employeeQueryService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    [HttpGet]
    [Route("GetAllSectionManager")]

    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllSectionManager ()
    {
        var sectionManager = await _employeeQueryService.ListAllByRole(UserRole.SectionManager);

        return Ok(sectionManager);
    }

    [HttpGet]
    [Route("GetAllShippingSupervisor")]

    public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> GetAllShippingSupervisor()
    {
        var supervisor = await _employeeQueryService.ListAllByRole(UserRole.ShippingSupervisor);

        return Ok(supervisor);
    }

    #endregion

    #region Command
    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        await _employeeCommandService.DeleteAsync(employeeId);

        return Ok("Employee deleted successfully");
    }
    
    #endregion

}