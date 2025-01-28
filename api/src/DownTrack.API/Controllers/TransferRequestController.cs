using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferRequestController : ControllerBase
{
    private readonly ITransferRequestQueryServices _transferRequestQueryService;
    private readonly ITransferRequestCommandServices _transferRequestCommandService;

    public TransferRequestController(ITransferRequestQueryServices transferRequestQueryServices,
                                     ITransferRequestCommandServices transferRequestCommandServices)
    {
        _transferRequestQueryService = transferRequestQueryServices;
        _transferRequestCommandService = transferRequestCommandServices;
    }

    #region Command

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateTransferRequest(TransferRequestDto transferRequest)
    {
        await _transferRequestCommandService.CreateAsync(transferRequest);

        return Ok("TransferRequest added successfully");
    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateTransferRequest(TransferRequestDto transferRequest)
    {
        var result = await _transferRequestCommandService.UpdateAsync(transferRequest);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{transferRequestId}")]

    public async Task<IActionResult> DeleteTransferRequest(int transferRequestId)
    {
        await _transferRequestCommandService.DeleteAsync(transferRequestId);

        return Ok("TransferRequest deleted successfully");
    }

    #endregion

    #region Query

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedUser([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _transferRequestQueryService.GetPagedResultAsync(paged);

        return Ok(result);

    }

    [HttpGet]
    [Route("{transferRequestId}")]

    public async Task<ActionResult<GetTransferRequestDto>> GetTransferRequestById(int transferRequestId)
    {
        var result = await _transferRequestQueryService.GetByIdAsync(transferRequestId);

        if (result == null)
            return NotFound($"TransferRequest with ID {transferRequestId} not found");

        return Ok(result);

    }
    
    [HttpGet]
    [Route("GetByArrivalDepartment/{arrivalDepartmentId}")]

    public async Task<IActionResult> GetTransferRequestByDepartment(int arrivalDepartmentId, [FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _transferRequestQueryService.GetPagedRequestsofArrivalDepartmentAsync(arrivalDepartmentId, paged);

        return Ok(result);

    }


    #endregion


}

