using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers
{
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

        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartents()
        {
            var results = await _departmentService.ListAsync();

            return Ok(results);

        }

        [HttpPut]
        [Route("PUT")]

        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDto department)
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

}