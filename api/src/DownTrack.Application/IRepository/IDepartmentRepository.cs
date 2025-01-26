using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;
public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<bool> ExistsByNameAndSectionAsync (string departmentName , int sectionId);
    Task<bool> ExistsByIdAndSectionAsync (int departmentId, int sectionId);
    Task<bool> ExistsByNameAndSectionNameAsync (string departmentName, string sectionName);
    Task<IEnumerable<Department>> GetDepartmentsBySectionIdAsync (int sectionId);
}
