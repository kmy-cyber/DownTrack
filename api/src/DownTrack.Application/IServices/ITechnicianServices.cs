
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;

public interface ITechnicianServices : IGenericService<TechnicianDto>
{
    Task<PagedResultDto<TechnicianDto>> GetPagedResultAsync(PagedRequestDto paged);
}