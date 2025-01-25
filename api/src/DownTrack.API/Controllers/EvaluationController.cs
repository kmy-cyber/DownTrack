using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DownTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly IEvaluationServices _evaluationService;

    public EvaluationController(IEvaluationServices evaluationServices)
    {
        _evaluationService = evaluationServices;
    }

    [HttpPost]
    [Route("POST")]

    public async Task<IActionResult> CreateEvaluation(EvaluationDto evaluation)
    {
        await _evaluationService.CreateAsync(evaluation);

        return Ok("Evaluation added successfully");
    }



    [HttpGet]
    [Route("GET")]

    public async Task<ActionResult<EvaluationDto>> GetUserById(int evaluationId)
    {
        var result = await _evaluationService.GetByIdAsync(evaluationId);

        if (result == null)
            return NotFound($"Evaluation with ID {evaluationId} not found");

        return Ok(result);

    }


    [HttpGet]
    [Route("GetPaged")]

    public async Task<IActionResult> GetPagedEvaluation ([FromQuery]PagedRequestDto paged)
    {
        paged.BaseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        var result = await _evaluationService.GetPagedResultAsync(paged);
        
        return Ok (result);
        
    }

    [HttpPut]
    [Route("PUT")]

    public async Task<IActionResult> UpdateEvaluation(EvaluationDto evaluation)
    {
        var result = await _evaluationService.UpdateAsync(evaluation);
        return Ok(result);
    }

    [HttpDelete]
    [Route("DELETE")]

    public async Task<IActionResult> DeleteEvaluation(int evaluationId)
    {
        await _evaluationService.DeleteAsync(evaluationId);

        return Ok("Evaluation deleted successfully");
    }
}