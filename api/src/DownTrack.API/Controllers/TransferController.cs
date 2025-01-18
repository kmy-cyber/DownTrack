using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferServices _transferService;

        public TransferController(ITransferServices transferServices)
        {
            _transferService = transferServices;
        }

        [HttpPost]
        [Route("POST")]

        public async Task<IActionResult> CreateTransfer(TransferDto transfer)
        {
            await _transferService.CreateAsync(transfer);

            return Ok("Transfer added successfully");
        }

        [HttpGet]
        [Route("GET_ALL")]

        public async Task<ActionResult<IEnumerable<Transfer>>> GetAllTransfer()
        {
            var results = await _transferService.ListAsync();
            
            return Ok(results);

        }
        

        [HttpGet]
        [Route("{transferId}")]

        public async Task<ActionResult<Transfer>> GetTransferById(int transferId)
        {
            var result = await _transferService.GetByIdAsync(transferId);

            if (result == null)
                return NotFound($"Transfer with ID {transferId} not found");

            return Ok(result);

        }

        [HttpPut]
        [Route("PUT")]

        public async Task<IActionResult> UpdateTransfer(TransferDto transfer)
        {
            var result = await _transferService.UpdateAsync(transfer);
            
            return Ok(result);
        }

        [HttpDelete]
        [Route("{transferId}")]

        public async Task<IActionResult> DeleteTransfer(int transferId)
        {
            await _transferService.DeleteAsync(transferId);

            return Ok("Transfer deleted successfully");
        }


    }

}