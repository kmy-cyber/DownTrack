using DownTrack.Application.DTO;
<<<<<<< HEAD
using DownTrack.Application.DTO.Paged;
=======
>>>>>>> fc2
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;
public interface ITransferRequestQueryServices : IGenericQueryService<TransferRequest,GetTransferRequestDto>
{
Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int receptorId, PagedRequestDto paged);

}