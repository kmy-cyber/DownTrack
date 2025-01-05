using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;
public interface IDepartmentRepository : IGenericRepository<Department>
{

    //puede devolver null
    public Task<Department> GetByIdAndSectionIdAsync(int departmentId, int sectionId);

    public  Task DeleteAsync(Department department);


}
