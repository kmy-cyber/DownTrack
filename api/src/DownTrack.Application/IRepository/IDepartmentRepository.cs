using DownTrack.Domain.Entities;
using System.Linq.Expressions;

namespace DownTrack.Application.IRepository;
public interface IDepartmentRepository : IGenericRepository<Department>
{

    Task<Department> GetByIdAndSectionIdAsync(int departmentId, int sectionId);

    Task DeleteAsync(int departmentId, int sectionId);

    Task<Department> GetByNameAsync(string name);

}
