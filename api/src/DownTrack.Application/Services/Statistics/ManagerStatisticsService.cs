
using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using DownTrack.Domain.Roles;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Application.Services.Statistics;


public class SectionManagerStatisticsService : ISectionManagerStatisticsService
{

    private readonly IUnitOfWork _unitOfWork;

    public SectionManagerStatisticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ManagerStatisticsDto> GetStatisticsBySectionManager(int managerId)
    {
        var manager = await _unitOfWork.GetRepository<Employee>()
                                      .GetByIdAsync(managerId);

        if (manager.UserRole != UserRole.SectionManager.ToString())
            throw new Exception($"User {managerId} is not a SectionManager");

        var transferRequestsMade = await _unitOfWork.GetRepository<TransferRequest>()
                                                    .GetAllByItems(tr => tr.SectionManagerId == managerId)
                                                    .CountAsync();

        var transferRequestsCompleted = await _unitOfWork.GetRepository<TransferRequest>()
                                                         .GetAllByItems(tr => tr.SectionManagerId == managerId,
                                                                        tr => tr.Status == TransferRequestStatus.Accepted.ToString())
                                                        .CountAsync();


        var numberOfDepartmentsInSection = await _unitOfWork.DepartmentRepository
                                                            .GetAllByItems(d => d.Section.SectionManagerId == managerId)
                                                            .CountAsync();

        var evaluationByType = await _unitOfWork.GetRepository<Evaluation>()
                                                .GetAllByItems(ev => ev.SectionManagerId == managerId)
                                                .GroupBy(ev => ev.Description)
                                                .Select(g => new { Description = g.Key, Count = g.Count() })
                                                .ToListAsync();

        var equipmentByStatus = await _unitOfWork.GetRepository<Equipment>()
                                                 .GetAllByItems(e => e.Department.Section.SectionManagerId == managerId)
                                                 .GroupBy(e => e.Status)
                                                 .Select(g => new { Status = g.Key, Count = g.Count() })
                                                 .ToListAsync();

        return new ManagerStatisticsDto
        {
            TransferRequestsMade = transferRequestsMade,
            TransferRequestsCompleted = transferRequestsCompleted,
            NumberOfDepartmentsInSection = numberOfDepartmentsInSection,
            EvaluationsByType = evaluationByType.ToDictionary(ev => ev.Description, ev => ev.Count),
            EquipmentsByStatus = equipmentByStatus.ToDictionary(e => e.Status, e => e.Count)
        };
    }
}