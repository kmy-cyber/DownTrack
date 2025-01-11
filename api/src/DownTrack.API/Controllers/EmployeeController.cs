
using DownTrack.Application.DTO;
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

    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployee()
    {
        var results = await _employeeService.ListAsync();

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

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEmployee(int employeeId)
    {
        await _employeeService.DeleteAsync(employeeId);

        return Ok("Employee deleted successfully");
    }


}

