using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferRequestController : ControllerBase
{
    private readonly ITransferRequestServices _transferRequestService;

    public TransferRequestController(ITransferRequestServices transferRequestServices)
    {
        _transferRequestService = transferRequestServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateTransferRequest(TransferRequestDto transferRequest)
    {
        await _transferRequestService.CreateAsync(transferRequest);

        return Ok("TransferRequest added successfully");
    }

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedUser ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _transferRequestService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    [HttpGet]
    [Route("{transferRequestId}")]

    public async Task<ActionResult<TransferRequestDto>> GetTransferRequestById(int transferRequestId)
    {
        var result = await _transferRequestService.GetByIdAsync(transferRequestId);

        if (result == null)
            return NotFound($"TransferRequest with ID {transferRequestId} not found");

        return Ok(result);

    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateTransferRequest(TransferRequestDto transferRequest)
    {
        var result = await _transferRequestService.UpdateAsync(transferRequest);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{transferRequestId}")]

    public async Task<IActionResult> DeleteTransferRequest(int transferRequestId)
    {
        await _transferRequestService.DeleteAsync(transferRequestId);

        return Ok("TransferRequest deleted successfully");
    }


}

