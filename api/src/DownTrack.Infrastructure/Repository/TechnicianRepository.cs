using DownTrack.Application.IRespository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}

}