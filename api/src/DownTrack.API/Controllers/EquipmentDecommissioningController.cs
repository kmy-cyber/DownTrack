using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentDecommissioningController : ControllerBase
{
    private readonly IEquipmentDecommissioningQueryServices _equipmentDecommissioningQueryServices;
    private readonly IEquipmentDecommissioningCommandServices _equipmentDecommissioningCommandServices;

    public EquipmentDecommissioningController(IEquipmentDecommissioningQueryServices equipmentDecommissioningQueryServices,
                                              IEquipmentDecommissioningCommandServices equipmentDecommissioningCommandServices)
    {
        _equipmentDecommissioningQueryServices = equipmentDecommissioningQueryServices;
        _equipmentDecommissioningCommandServices = equipmentDecommissioningCommandServices;
    }

    #region Command

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateEquipmentDecommissioning(EquipmentDecommissioningDto equipmentDecommissioning)
    {
        await _equipmentDecommissioningCommandServices.CreateAsync(equipmentDecommissioning);

        return Ok("Equipment Decommissioning added successfully");
    }



    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> UpdateEquipmentDecommissioning(EquipmentDecommissioningDto equipmentDecommissioning)
    {
        await _equipmentDecommissioningCommandServices.UpdateAsync(equipmentDecommissioning);

        return Ok("Equipment Decommissioning updated successfully");
    }

    [HttpDelete]
    [Route("{equipmentDecommissioningId}/delete")]
    public async Task<IActionResult> DeleteEquipmentDecommissioning(int equipmentDecommissioningId)
    {
        await _equipmentDecommissioningCommandServices.DeleteAsync(equipmentDecommissioningId);

        return Ok("Equipment Decommissioning deleted successfully");
    }

    [HttpPost]
    [Route("{equipmentDecommissioningId}/accept")]
    public async Task<IActionResult> AcceptDecommissioning(int equipmentDecommissioningId)
    {
        await _equipmentDecommissioningCommandServices.AcceptDecommissioningAsync(equipmentDecommissioningId);

        return Ok("Equipment Decommissioning accepted successfully");
    }

    #endregion

    #region Query

    [HttpGet]
    [Route("GET_ALL")]

    public async Task<ActionResult<IEnumerable<GetEquipmentDecommissioningDto>>> GetAllEquipmentDecommissioning()
    {
        var results = await _equipmentDecommissioningQueryServices.ListAsync();

        return Ok(results);

    }

    [HttpGet]
    [Route("Get_Paged_All")]
    public async Task<IActionResult> GetPagedAllDepartmentInSection([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentDecommissioningQueryServices.GetAllPagedResultAsync(paged);

        return Ok(result);

    }

    [HttpGet]
    [Route("Get_Paged_Accepted")]
    public async Task<IActionResult> GetPagedAcceptedDecommissioning([FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentDecommissioningQueryServices.GetAcceptedDecommissioning(paged);

        return Ok(result);

    }


    [HttpGet]
    [Route("{equipmentDecommissioningId}/GET_BY_ID")]
    public async Task<ActionResult<GetEquipmentDecommissioningDto>> GetEquipmentDecommissioningById(int equipmentDecommissioningId)
    {
        var result = await _equipmentDecommissioningQueryServices.GetByIdAsync(equipmentDecommissioningId);

        if (result == null)
            return NotFound($"Equipment Decommissioning with ID {equipmentDecommissioningId} not found");

        return Ok(result);

    }

    [HttpGet]
    [Route("Get_Paged_All_By_ReceptorId/{receptorId}")]
    public async Task<IActionResult> GetEquipmentDecomissioningByReceptorId(int receptorId, [FromQuery] PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _equipmentDecommissioningQueryServices.GetEquipmentDecomissioningOfReceptorAsync(receptorId, paged);

        return Ok(result);

    }

    [HttpGet]
    [Route("Get_Decomissions_By_Equipment_Id/{equipmentId}")]
    public async Task<ActionResult<GetEquipmentDecommissioningDto>> GetDecomissionsByEquipmentId(int equipmentId)
    {
        var decomissions = await _equipmentDecommissioningQueryServices.GetDecomissionByEquipmentIdAsync(equipmentId);

        return Ok(decomissions);
    }

    [HttpGet]
    [Route("Get_Decomissions_Last_Year")]

    public async Task<ActionResult<GetEquipmentDecommissioningDto>> GetDecomissionsLastYear([FromQuery] PagedRequestDto paged)
    {
        var decomissions = await _equipmentDecommissioningQueryServices.GetDecomissionLastYear(paged);

        return Ok(decomissions);
    }

    #endregion




}