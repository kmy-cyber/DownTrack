using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(DownTrackContext context) : base(context) {}
}
