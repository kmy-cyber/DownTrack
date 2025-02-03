using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TransferController : ControllerBase
{
    private readonly ITransferQueryServices _transferQueryService;
    private readonly ITransferCommandServices _transferCommandService;

    public TransferController(ITransferQueryServices transferQueryServices,
                             ITransferCommandServices transferCommandServices)
    {
        _transferQueryService = transferQueryServices;
        _transferCommandService = transferCommandServices;
    }

    #region Command

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateTransfer(TransferDto transfer)
    {
        await _transferCommandService.CreateAsync(transfer);

        return Ok("Transfer added successfully");
    }


    
    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateTransfer(TransferDto transfer)
    {
        var result = await _transferCommandService.UpdateAsync(transfer);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{transferId}")]

    public async Task<IActionResult> DeleteTransfer(int transferId)
    {
        await _transferCommandService.DeleteAsync(transferId);

        return Ok("Transfer deleted successfully");
    }

    #endregion

    #region Query

    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedUser ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _transferQueryService.GetAllPagedResultAsync(paged);
        
        return Ok (result);
        
    }


    [HttpGet]
    [Route("{transferId}")]

    public async Task<ActionResult<GetTransferDto>> GetTransferById(int transferId)
    {
        var result = await _transferQueryService.GetByIdAsync(transferId);

        if (result == null)
            return NotFound($"Transfer with ID {transferId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("Get_Transfers_Requested_By_Manager")]

        public async Task<IActionResult> GetPagedTransferByManager ([FromQuery]PagedRequestDto paged, int managerId)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _transferQueryService.GetPagedTransferRequestedbyManager(managerId, paged);
        
        return Ok (result);
        
    }

    [HttpGet]
    [Route("Get_Transfer_Between_Sections")]

    public async Task<ActionResult<GetTransferDto>> GetTransferBetweenSections([FromQuery]PagedRequestDto paged,int sectionId1, int sectionId2)
    {
        var result = await _transferQueryService.GetTransferBetweenSections(paged,sectionId1,sectionId2);

        return Ok(result);

    }
    #endregion


}

