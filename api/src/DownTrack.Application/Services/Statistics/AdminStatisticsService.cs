

using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services.Statistics;


public class AdminStatisticsService : IAdminStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;

    public AdminStatisticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AdminStatisticsDto> GetStatisticsForAdmins()
    {
        var employees = await _unitOfWork.GetRepository<Employee>()
                                         .GetAll()
                                         .CountAsync();

        var sections = await _unitOfWork.GetRepository<Section>()
                                        .GetAll()
                                        .CountAsync();
        var departaments = await _unitOfWork.DepartmentRepository.GetAll().CountAsync();

        var roles = await _unitOfWork.GetRepository<Employee>()
                                 .GetAll()
                                 .GroupBy(e => e.UserRole)
                                 .Select(g => new { Role = g.Key, Count = g.Count() })
                                 .ToListAsync();

        var departmentsByMonth = await _unitOfWork.GetRepository<Department>()
                                             .GetAll()
                                             .Where(d => d.CreatedDate >= DateTime.Now.AddYears(-1))
                                             .GroupBy(d => new { d.CreatedDate.Month, d.CreatedDate.Year })
                                             .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                             .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                             .ToListAsync();

        var sectionsByMonth = await _unitOfWork.GetRepository<Section>()
                                          .GetAll()
                                          .Where(s => s.CreatedDate >= DateTime.Now.AddYears(-1))
                                          .GroupBy(s => new { s.CreatedDate.Month, s.CreatedDate.Year })
                                          .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                          .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                          .ToListAsync();

        return new AdminStatisticsDto
        {
            NumberEmployee = employees,
            NumberSections = sections,
            NumberDepartments = departaments,
            RolesStatistics = roles.ToDictionary(r => r.Role, r => r.Count),
            DepartmentsByMonth = departmentsByMonth.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count),
            SectionsByMonth = sectionsByMonth.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count)
        };
    }
}