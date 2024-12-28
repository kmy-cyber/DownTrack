using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using DownTrack.Infrastructure.Reposiory;

namespace DownTrack.Infrastructure.Repository;

public class EquipmentRepository : GenericRepository<Equipment>, IEquipmentRepository
{
    public EquipmentRepository(DownTrackContext context) : base(context) { }
}
