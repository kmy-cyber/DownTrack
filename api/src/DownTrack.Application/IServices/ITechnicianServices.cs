
using System.Linq.Expressions;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface ITechnicianServices : IGenericService<TechnicianDto>
{
    Task<PagedResultDto<TechnicianDto>> GetPagedResultWithFilterAsync(PagedRequestDto paged, Expression<Func<Technician,bool>> exp);
}