using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;

public interface ISectionServices : IGenericService<SectionDto>
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartments (int sectionId);
}