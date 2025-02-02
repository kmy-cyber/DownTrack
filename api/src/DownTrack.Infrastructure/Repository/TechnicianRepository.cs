using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;


namespace DownTrack.Infrastructure.Repository;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}


}