using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;

public interface IDoneMaintenanceQueryServices : IGenericQueryService<GetDoneMaintenanceDto>
{
    Task<PagedResultDto<GetDoneMaintenanceDto>> GetByTechnicianIdAsync (PagedRequestDto paged, int technicianId);
}