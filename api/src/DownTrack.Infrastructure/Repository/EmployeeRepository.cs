

using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(DownTrackContext context) : base(context) { }
}