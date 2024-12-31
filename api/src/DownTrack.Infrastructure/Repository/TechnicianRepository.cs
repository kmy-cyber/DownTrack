using DownTrack.Application.IRespository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}

    public override Task<Technician> CreateAsync(Technician element, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(element.Role);
        return base.CreateAsync(element, cancellationToken);
    }
}