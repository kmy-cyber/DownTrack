using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;
public interface IDepartmentServices : IGenericService<DepartmentDto>
{
    Task DeleteAsync(int id, int SectionId);
    
}