
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;

public interface ISectionQueryServices : IGenericQueryService<GetSectionDto>
{
    //Task<IEnumerable<DepartmentDto>> GetAllDepartments (int sectionId);
    Task<PagedResultDto<GetSectionDto>> GetSectionsByManageAsync(PagedRequestDto paged, int sectionManagerId);
}