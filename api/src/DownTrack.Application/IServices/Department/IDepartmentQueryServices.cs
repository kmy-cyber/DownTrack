using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;

public interface IDepartmentQueryServices : IGenericQueryService<GetDepartmentDto>
{
    Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsInSection(int sectionId);
    Task<PagedResultDto<GetDepartmentDto>> GetPagedAllDepartmentsInSection(PagedRequestDto paged, int sectionId);
}