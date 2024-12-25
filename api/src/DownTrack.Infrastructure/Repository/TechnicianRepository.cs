using DownTrack.Application.IRespository;
using DownTrack.Domain.Enitites;

namespace DownTrack.Infrastructure.Reposiory;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}
}