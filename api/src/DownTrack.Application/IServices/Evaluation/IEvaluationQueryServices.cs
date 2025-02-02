using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IEvaluationQueryServices : IGenericQueryService<Evaluation,GetEvaluationDto>
{
    Task<PagedResultDto<GetEvaluationDto>>  GetEvaluationByTechnicianAsync(PagedRequestDto paged,int technicianId);
}
