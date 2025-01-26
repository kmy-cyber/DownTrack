using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;

public interface IDepartmentQueryServices : IGenericQueryService<GetDepartmentDto>
{
    Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsInSection (int sectionId);
}