using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;
public interface IDepartmentRepository : IGenericRepository<Department>
{
    public Task<Department?> GetByIdAndSectionIdAsync(int departmentId, int sectionId);

}
