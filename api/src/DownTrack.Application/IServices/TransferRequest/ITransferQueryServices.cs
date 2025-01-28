using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;
public interface ITransferQueryServices : IGenericQueryService<Transfer,GetTransferDto>
{
    
}