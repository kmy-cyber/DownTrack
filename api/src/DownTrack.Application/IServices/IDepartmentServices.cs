using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;
public interface IDepartmentServices
{
    Task DeleteAsync(int id, int SectionId);

    public Task<DepartmentDto> CreateAsync(DepartmentDto dto);

    public  Task DeleteAsync(int dto);
    public  Task<IEnumerable<DepartmentPresentationDto>> ListAsync();
    public  Task<DepartmentDto> UpdateAsync(DepartmentDto dto);
    public  Task<DepartmentDto> GetByIdAsync(int departmentDto);


}