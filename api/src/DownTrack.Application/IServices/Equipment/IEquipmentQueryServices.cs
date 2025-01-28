using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;

public interface IEquipmentQueryServices : IGenericQueryService<GetEquipmentDto>
{

    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionManagerIdAsync(PagedRequestDto paged , int sectionManagerId);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionIdAsync(PagedRequestDto paged , int sectionId);
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByDepartmentIdAsync(PagedRequestDto paged , int departmentId);
    

}
