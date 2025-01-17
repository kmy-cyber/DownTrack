using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class EquipmentDecommissioningRepository : GenericRepository<EquipmentDecommissioning>, IEquipmentDecommissioningRepository
{
    public EquipmentDecommissioningRepository(DownTrackContext context) : base(context) {}
}