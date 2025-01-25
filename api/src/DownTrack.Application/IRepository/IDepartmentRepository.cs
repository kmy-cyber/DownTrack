using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;
public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<bool> ExistsByNameAndSectionAsync (string departmentName , int sectionId);

    Task<IEnumerable<Department>> GetDepartmentsBySectionIdAsync (int sectionId);
}
