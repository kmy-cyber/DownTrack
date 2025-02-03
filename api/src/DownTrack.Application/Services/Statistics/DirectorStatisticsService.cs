
using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Application.Services.Statistics;


public class DirectorStatisticsService : IDirectorStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;

    public DirectorStatisticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DirectorStatisticsDto> GetStatisticsByDirector()
    {
        var numberOfEquipments = await _unitOfWork.GetRepository<Equipment>()
                                                 .GetAll()
                                                 .CountAsync();

        var numberOfTotalMaintenances = await _unitOfWork.GetRepository<DoneMaintenance>()
                                                         .GetAll()
                                                         .CountAsync();

        var numberOfCompletedMaintenances = await _unitOfWork.GetRepository<DoneMaintenance>()
                                                             .GetAllByItems(dm => dm.Finish)
                                                             .CountAsync();

        var acceptedDecommissions = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                          .GetAllByItems(ed => ed.Status == DecommissioningStatus.Accepted.ToString(),
                                                         ed => ed.Date >= DateTime.Now.AddYears(-1))
                                          .GroupBy(s => new { s.Date.Month, s.Date.Year })
                                          .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                          .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                          .ToListAsync();

        var transferByMonth = await _unitOfWork.GetRepository<Transfer>()
                                          .GetAllByItems(t => t.Date >= DateTime.Now.AddYears(-1))
                                          .GroupBy(s => new { s.Date.Month, s.Date.Year })
                                          .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                          .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                          .ToListAsync();


        var maintenanceCostByMonth = await _unitOfWork.GetRepository<DoneMaintenance>()
                                                      .GetAllByItems(dm => dm.Finish)
                                                      .GroupBy(dm => new { dm.Date.Month, dm.Date.Year })
                                                      .Select(g => new
                                                      {
                                                          g.Key.Month,
                                                          g.Key.Year,
                                                          TotalCost = g.Sum(dm => dm.Cost)
                                                      })
                                                      .ToListAsync();
        return new DirectorStatisticsDto
        {
            NumberOfEquipments = numberOfEquipments,
            NumberOfTotalMaintenances = numberOfTotalMaintenances,
            NumberOfCompletedMaintenances = numberOfCompletedMaintenances,
            TransfersByMonth = transferByMonth.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count),
            AcceptedDecommissionsByMonth = acceptedDecommissions.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count),
            MaintenanceCostByMonth = maintenanceCostByMonth.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.TotalCost)
        };
    }
}