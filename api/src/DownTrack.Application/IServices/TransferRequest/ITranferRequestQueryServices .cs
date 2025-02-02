using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;
public interface ITransferRequestQueryServices : IGenericQueryService<TransferRequest,GetTransferRequestDto>
{
Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int receptorId, PagedRequestDto paged);

Task<PagedResultDto<GetTransferRequestDto>>GetTransferRequestByEquipmentIdAsync (PagedRequestDto paged, int equipmentId);

}