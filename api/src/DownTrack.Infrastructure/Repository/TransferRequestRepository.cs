using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
namespace DownTrack.Infrastructure.Repository;

public class TransferRequestRepository : GenericRepository<TransferRequest>, ITransferRequestRepository
{
    public TransferRequestRepository(DownTrackContext context) : base(context) { }

}