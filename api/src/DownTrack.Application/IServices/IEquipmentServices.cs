using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;


public interface IEquipmentServices : IGenericService<EquipmentDto>
{
    public Task<PagedResultDto<Equipment>> GetPagedEquipmentsBySectionManagerIdAsync(
        int sectionManagerId,
        PagedRequestDto pagedRequest);

}
