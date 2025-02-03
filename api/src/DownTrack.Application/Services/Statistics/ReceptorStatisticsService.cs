using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using Microsoft.EntityFrameworkCore;


namespace DownTrack.Application.Services.Statistics;


public class ReceptorStatisticsService : IReceptorStatisticsService
{

    private readonly IUnitOfWork _unitOfWork;

    public ReceptorStatisticsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ReceptorStatisticsDto> GetStatisticsByReceptor(int receptorId)
    {
        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
                                        .GetByIdAsync(receptorId);

        // numero de solicitudes de transferencias que han llegado a su departamento
        var pendingTransfers = await _unitOfWork.GetRepository<TransferRequest>()
                                                .GetAllByItems(tr => tr.ArrivalDepartmentId == receptor.DepartmentId,
                                                              tr => tr.Status == "Pending")
                                                .CountAsync();

        // numero de bajas asignadas
        var pendingDecommissions = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                    .GetAllByItems(ed => ed.ReceptorId == receptorId,
                                                                   ed => ed.Status == "Pending")
                                                    .CountAsync();

        //total de equipos en su departamento
        var totalEquipments = await _unitOfWork.GetRepository<Equipment>()
                                               .GetAllByItems(e => e.DepartmentId == receptor.DepartmentId)
                                               .CountAsync();

        // total de bajas aceptadas por mes ultimamente
        var acceptedDecommissions = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                          .GetAllByItems(ed => ed.ReceptorId == receptorId,
                                                         ed => ed.Status == DecommissioningStatus.Accepted.ToString(),
                                                         ed => ed.Date >= DateTime.Now.AddYears(-1))
                                          .GroupBy(s => new { s.Date.Month, s.Date.Year })
                                          .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                          .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                          .ToListAsync();

        // traslados registrados por mes

        var proccessedTransfer = await _unitOfWork.GetRepository<Transfer>()
                                                  .GetAllByItems(t => t.EquipmentReceptorId == receptorId,
                                                                 t => t.Date >= DateTime.Now.AddYears(-1))
                                                  .GroupBy(s => new { s.Date.Month, s.Date.Year })
                                                  .Select(g => new { g.Key.Month, g.Key.Year, Count = g.Count() })
                                                  .OrderBy(g => g.Year).ThenBy(g => g.Month)
                                                  .ToListAsync();

        return new ReceptorStatisticsDto
        {
            PendingTransfers = pendingTransfers,
            PendingDecommissions = pendingDecommissions,
            TotalEquipments = totalEquipments,
            AcceptedDecommissionsPerMonth = acceptedDecommissions.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count),
            ProcessedTransfersPerMonth = proccessedTransfer.ToDictionary(g => $"{g.Month}-{g.Year}", g => g.Count)
        };
    }


}