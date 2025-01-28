using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;
public interface ITransferRequestQueryServices : IGenericQueryService<GetTransferRequestDto>
{
Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int receptorId, PagedRequestDto paged);

}