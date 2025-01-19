using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class DoneMaintenanceRepository : GenericRepository<DoneMaintenance>, IDoneMaintenanceRepository
{
    public DoneMaintenanceRepository(DownTrackContext context) : base(context) { }
}