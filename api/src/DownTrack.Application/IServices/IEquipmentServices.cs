using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;


public interface IEquipmentServices : IGenericService<EquipmentDto>
{
    public Task<PagedResultDto<Equipment>> GetPagedEquipmentsBySectionManagerIdAsync(
        int sectionManagerId,
        PagedRequestDto pagedRequest);

    public Task<PagedResultDto<Equipment>> GetPagedEquipmentsBySectionIdAsync(
        int sectionId,
        PagedRequestDto pagedRequest);
    public Task<PagedResultDto<Equipment>> GetPagedEquipmentsByDepartmentIdAsync(
        int departmentId,
        PagedRequestDto pagedRequest);

}
