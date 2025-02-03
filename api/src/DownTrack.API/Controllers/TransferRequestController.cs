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
        var transfer = await _transferRequestCommandService.UpdateAsync(transferRequest);

        return Ok(transfer);
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

    public async Task<IActionResult> GetPagedTransfer([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var transfer = await _transferRequestQueryService.GetAllPagedResultAsync(paged);
        
        return Ok (transfer);
        
    }

    [HttpGet]
    [Route("{transferRequestId}")]

    public async Task<ActionResult<GetTransferRequestDto>> GetTransferRequestById(int transferRequestId)
    {
        var transfer = await _transferRequestQueryService.GetByIdAsync(transferRequestId);

        if (transfer == null)
            return NotFound($"TransferRequest with ID {transferRequestId} not found");

        return Ok(transfer);

    }
    
    [HttpGet]
    [Route("GetByArrivalDepartment/{receptorId}")]

    public async Task<IActionResult> GetTransferRequestByDepartment(int receptorId, [FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var transfer = await _transferRequestQueryService.GetPagedRequestsofArrivalDepartmentAsync(receptorId, paged);

        return Ok(transfer);

    }

    [HttpGet]
    [Route("Get_TransferRequest_By_EquipmentId")]

    public async Task<IActionResult> GetTransferRequestByEquipmentId ([FromQuery] PagedRequestDto paged, int equipmentId)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var transfers = await _transferRequestQueryService.GetTransferRequestByEquipmentIdAsync (paged,equipmentId);

        return Ok(transfers);
    }


    #endregion


}

