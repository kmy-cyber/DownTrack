

using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicianController : ControllerBase
    {
        private readonly ITechnicianServices _technicianService;

        public TechnicianController(ITechnicianServices technicianServices)
        {
            _technicianService = technicianServices;
        }

        [HttpPost]
        [Route("POST")]

        public async Task<IActionResult> CreateTechnician(TechnicianDto technician)
        {
            await _technicianService.CreateAsync(technician);

            return Ok("Technician added successfully");
        }

        [HttpGet]
        [Route("GET_ALL")]

        public async Task<ActionResult<IEnumerable<Technician>>> GetAllTechnician()
        {
            var results = await _technicianService.ListAsync();

            return Ok(results);

        }

        [HttpPut]
        [Route("PUT")]

        public async Task<IActionResult> UpdateTechnician(TechnicianDto technician)
        {
            var result = await _technicianService.UpdateAsync(technician);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Delete")]

        public async Task<IActionResult> DeleteTechnician(int technicianId)
        {
            await _technicianService.DeleteAsync(technicianId);

            return Ok("Technician deleted successfully");
        }


    }

}