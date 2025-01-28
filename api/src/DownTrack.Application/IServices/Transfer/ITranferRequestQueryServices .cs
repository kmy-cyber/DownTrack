using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;
public interface ITransferRequestQueryServices : IGenericQueryService<TransferRequest,GetTransferRequestDto>
{
    
}