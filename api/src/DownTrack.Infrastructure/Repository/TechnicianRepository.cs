using DownTrack.Application.IRespository;
using DownTrack.Domain.Enitites;
using DownTrack.Infrastructure.Repository;

namespace DownTrack.Infrastructure.Reposiory;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}
}