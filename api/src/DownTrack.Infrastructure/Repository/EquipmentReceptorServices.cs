using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class EquipmentReceptorRepository : GenericRepository<EquipmentReceptor>, IEquipmentReceptorRepository
{
    public EquipmentReceptorRepository(DownTrackContext context) : base(context){}


}