using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly IEvaluationQueryServices _evaluationQueryService;
    private readonly IEvaluationCommandServices _evaluationCommandService;

    public EvaluationController(IEvaluationQueryServices evaluationQueryServices,
                                IEvaluationCommandServices evaluationCommandServices)
    {
        _evaluationQueryService = evaluationQueryServices;
        _evaluationCommandService = evaluationCommandServices;

    }

    #region Command
    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateEvaluation(EvaluationDto evaluation)
    {
        await _evaluationCommandService.CreateAsync(evaluation);

        return Ok("Evaluation added successfully");
    }

     [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateEvaluation(EvaluationDto evaluation)
    {
        var result = await _evaluationCommandService.UpdateAsync(evaluation);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEvaluation(int evaluationId)
    {
        await _evaluationCommandService.DeleteAsync(evaluationId);

        return Ok("Evaluation deleted successfully");
    }

    #endregion

    #region Query
    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<EvaluationDto>> GetUserById(int evaluationId)
    {
        var result = await _evaluationQueryService.GetByIdAsync(evaluationId);

        if (result == null)
            return NotFound($"Evaluation with ID {evaluationId} not found");

        return Ok(result);

    }


    [HttpPost]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEvaluation (PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _evaluationQueryService.GetAllPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    #endregion
   
}