using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices
{
    public interface IDepartmentServices 
    {
    Task<DepartmentDto> CreateAsync (DepartmentDto dto);
    Task<UpdateDepartmentDto> UpdateAsync (UpdateDepartmentDto dto);
    Task<IEnumerable<DepartmentDto>> ListAsync ();
    Task DeleteAsync (int dto);    }
}