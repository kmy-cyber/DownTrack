using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface ISectionServices : IGenericService<SectionDto>
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartments(int sectionId);

    public Task<PagedResultDto<Section>> GetPagedSectionsByManagerIdAsync(
        int managerId, PagedRequestDto pagedRequest);
}