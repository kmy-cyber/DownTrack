using DownTrack.Application.DTO.Statistics;
using DownTrack.Application.IServices.Statistics;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using DownTrack.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services.Statistics;

public class EmployeeStatisticsServices : IEmployeeStatisticsService
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeStatisticsServices (IUnitOfWork unitOfWork)
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


    public async Task<ManagerStatisticsDto> GetStatisticsBySectionManager(int managerId)
    {
        var manager = await _unitOfWork.GetRepository<Employee>()
                                       .GetByIdAsync(managerId);

        if(manager.UserRole != UserRole.SectionManager.ToString())
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
                                                 .GetAllByItems(e=> e.Department.Section.SectionManagerId == managerId)
                                                 .GroupBy(e=> e.Status)
                                                 .Select(g=> new {Status = g.Key, Count = g.Count()})
                                                 .ToListAsync();

        return new ManagerStatisticsDto
        {
            TransferRequestsMade = transferRequestsMade,
            TransferRequestsCompleted = transferRequestsCompleted,
            NumberOfDepartmentsInSection = numberOfDepartmentsInSection,
            EvaluationsByType = evaluationByType.ToDictionary(ev=> ev.Description,ev=> ev.Count),
            EquipmentsByStatus = equipmentByStatus.ToDictionary(e => e.Status, e => e.Count)
        };
    }
}