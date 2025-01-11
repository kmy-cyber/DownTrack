

namespace DownTrack.Application.IServices;

public interface IGenericService<TDto>
{
    Task<TDto> CreateAsync(TDto dto);
    Task<TDto> UpdateAsync(TDto dto);
    Task<IEnumerable<TDto>> ListAsync();
    Task DeleteAsync(int dto);
    Task<TDto> GetByIdAsync(int dto);
}