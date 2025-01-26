using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;


namespace DownTrack.Application.IServices;
public interface IDepartmentServices : IGenericService<DepartmentDto> 
{
        public  Task<PagedResultDto<Department>> GetPagedDepartmentsBySectionIdAsync(
    int sectionId,
    PagedRequestDto pagedRequest);

}