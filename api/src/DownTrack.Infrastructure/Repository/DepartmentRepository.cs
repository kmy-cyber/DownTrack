using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(DownTrackContext context) : base(context) { }

    public async Task<Department?> GetByIdAndSectionIdAsync(int departmentId, int sectionId)
    {
        return await entity.FirstOrDefaultAsync(d => d.Id == departmentId && d.SectionId == sectionId);
    }

}
