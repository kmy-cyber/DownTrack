using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Application.Services.Statistics;


public class TechnicianStatisticsService : ITechnicianStatisticsService
{

    private readonly IUnitOfWork _unitOfWork;

    public TechnicianStatisticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<TechnicianStatisticsDto> GetStatisticsByTechnician(int technicianId)
    {
        var technician = await _unitOfWork.GetRepository<Technician>()
                                          .GetByIdAsync(technicianId);

        var maintenanceFinish = await _unitOfWork.GetRepository<DoneMaintenance>()
                                                 .GetAllByItems(
                                                                dm => dm.Finish,
                                                                dm => dm.TechnicianId == technicianId
                                                            ).CountAsync();

        var maintenanceProgress = await _unitOfWork.GetRepository<DoneMaintenance>()
                                                 .GetAllByItems(
                                                                dm => !dm.Finish,
                                                                dm => dm.TechnicianId == technicianId
                                                            ).CountAsync();

        var decomissionsTotal = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                            .GetAllByItems(ed => ed.TechnicianId == technicianId)
                                            .CountAsync();

        //mantenimientos realizados por mes

        var maintenances = await _unitOfWork.GetRepository<DoneMaintenance>()
                                          .GetAll()
                                          .Where(s => s.TechnicianId == technicianId && s.Date >= DateTime.Now.AddYears(-1))
                                          .GroupBy(s => new { s.Date.Month, s.Date.Year })
                                          .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                          .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                          .ToListAsync();


        //bajas propuestas por mes

        var decomissions = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                          .GetAll()
                                          .Where(s => s.TechnicianId == technicianId && s.Date >= DateTime.Now.AddYears(-1))
                                          .GroupBy(s => new { s.Date.Month, s.Date.Year })
                                          .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                          .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                          .ToListAsync();


        //de los equipos las listas por status

        var equipments = await _unitOfWork.GetRepository<Equipment>()
                                         .GetAll()
                                         .GroupBy(e => e.Status)
                                         .Select(g => new { Status = g.Key, Count = g.Count() })
                                         .ToListAsync();

        return new TechnicianStatisticsDto
        {
            Id = technicianId,
            Maintenances = maintenanceFinish,
            MaintenancesInProgress = maintenanceProgress,
            Decomissions = decomissionsTotal,
            MaintenanceByMonth = maintenances.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count),
            DecomissionsByMonth = decomissions.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count),
            EquipmentByStatus = equipments.ToDictionary(e => e.Status, e => e.Count)

        };
    }
}