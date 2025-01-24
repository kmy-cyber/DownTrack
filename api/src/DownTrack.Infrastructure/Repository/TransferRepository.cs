using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class TransferRepository : GenericRepository<Transfer>, ITransferRepository
{

    public TransferRepository(DownTrackContext context) : base(context)
    {
       
    }

}
