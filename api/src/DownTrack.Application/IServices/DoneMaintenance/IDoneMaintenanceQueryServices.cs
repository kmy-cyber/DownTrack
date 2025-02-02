using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IDoneMaintenanceQueryServices : IGenericQueryService<DoneMaintenance,GetDoneMaintenanceDto>
{
    Task<PagedResultDto<GetDoneMaintenanceDto>> GetByTechnicianIdAsync (PagedRequestDto paged, int technicianId);
    Task<PagedResultDto<GetDoneMaintenanceDto>> GetMaintenanceByTechnicianStatusAsync(PagedRequestDto paged,int technicianId, bool isFinish);

    Task<PagedResultDto<GetDoneMaintenanceDto>> GetMaintenanceByEquipmentIdAsync(PagedRequestDto paged,int equipmentId);
}