using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DownTrack.Infrastructure.Repository;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{


    public DepartmentRepository(DownTrackContext context) : base(context)
    {

    }

    public async Task<Department> GetByIdAndSectionIdAsync(int departmentId, int sectionId)
    {
        var result = await _entity.FirstOrDefaultAsync(d => d.Id == departmentId && d.SectionId == sectionId);
        if (result == null)
            throw new KeyNotFoundException($"No se encontró una entidad con el ID '{departmentId} y {sectionId}'.");
        return result;
    }

    public async Task DeleteAsync(int departmentId, int sectionId)
    {
        var result = await GetByIdAndSectionIdAsync(departmentId, sectionId);

        _entity.Remove(result);
    }

    public async Task<Department> GetByNameAsync(string name)
    {
        var department = await _entity.SingleOrDefaultAsync(d => d.Name == name);

        if (department is null)
            throw new Exception();

        return department;
    }

}
