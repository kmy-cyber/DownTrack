using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class MaintenaceRepository : GenericRepository<Maintenance>, IMaintenanceRepository
{
    public MaintenaceRepository(DownTrackContext context) : base(context) { }
}
