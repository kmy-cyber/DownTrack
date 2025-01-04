using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    private readonly DownTrackContext? _context;
    public DepartmentRepository(DownTrackContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

    }

    public async Task<Department?> GetByIdAndSectionIdAsync(int departmentId, int sectionId)
    {
        return await entity.FirstOrDefaultAsync(d => d.Id == departmentId && d.SectionId == sectionId);
    }

    public async Task DeleteAsync(Department department)
    {
        entity.Remove(department);
        await _context.SaveChangesAsync();
    }
}
