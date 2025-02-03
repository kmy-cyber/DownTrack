using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;
public interface ITransferQueryServices : IGenericQueryService<Transfer, GetTransferDto>
{

    Task<PagedResultDto<GetTransferDto>> GetPagedTransferRequestedbyManager(int managerId, PagedRequestDto paged);

}