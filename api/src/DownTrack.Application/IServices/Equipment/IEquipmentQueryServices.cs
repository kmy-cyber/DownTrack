using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IEquipmentQueryServices : IGenericQueryService<Equipment, GetEquipmentDto>
{

    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionManagerIdAsync(PagedRequestDto paged, int sectionManagerId);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionIdAsync(PagedRequestDto paged, int sectionId);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByDepartmentIdAsync(PagedRequestDto paged, int departmentId);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByNameAsync(PagedRequestDto aged, string equipmentName);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByNameAndSectionManagerAsync(PagedRequestDto paged, string equipmentName, int sectionManagerId);
    Task<PagedResultDto<GetEquipmentDto>> GetActiveEquipment(PagedRequestDto paged);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsWith3MaintenancesAsync(PagedRequestDto paged);
}
