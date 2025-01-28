using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IDepartmentQueryServices : IGenericQueryService<Department,GetDepartmentDto>
{
    Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsInSection(int sectionId);
    Task<PagedResultDto<GetDepartmentDto>> GetPagedAllDepartmentsInSection(PagedRequestDto paged, int sectionId);
}