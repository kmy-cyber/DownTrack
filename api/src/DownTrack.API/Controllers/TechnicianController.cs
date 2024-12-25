

using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
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
        [Route("create")]

        public async Task<IActionResult> CreateTechnician(TechnicianDto technician)
        {
            await _technicianService.CreateAsync(technician);

            return Ok("Tecnico agregado correctamente");
        }

    }
}