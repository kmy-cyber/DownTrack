
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.Interfaces;

public interface IPaginationService<TEntity>
{
    Task <PagedResultDto<TDto>> ApplyPaginationAsync<TDto>(IQueryable<TEntity>query,PagedRequestDto pageRequest);
    
}