using DownTrack.Application.DTO;


namespace DownTrack.Application.IServices
{
    public interface IDepartmentServices
    {
        Task<DepartmentDto> CreateAsync(DepartmentDto dto);
        Task<DepartmentDto> UpdateAsync(DepartmentDto dto);
        Task<IEnumerable<DepartmentDto>> ListAsync();
        Task DeleteAsync(int id, int SectionId);

    }
}