
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface ISectionQueryServices : IGenericQueryService<Section,GetSectionDto>
{
    Task<PagedResultDto<GetSectionDto>> GetSectionsByManagerAsync(PagedRequestDto paged, int sectionManagerId);

    Task<GetSectionDto> GetSectionByNameAsync(string sectionName);
}