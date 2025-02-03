
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Statistics;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.IServices;

public interface IEmployeeQueryServices : IGenericQueryService<Employee,GetEmployeeDto>
{
    Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role);

    Task<GetEmployeeDto> GetByUserNameAsync(string employeeUserName);

    Task<AdminStatisticsDto> GetStatisticsForAdmins();
    Task<TechnicianStatisticsDto> GetStatisticsByTechnician(int technicianId);
    Task<ReceptorStatisticsDto> GetStatisticsByReceptor(int receptorId);

    Task<DirectorStatisticsDto> GetStatisticsByDirector();

    Task<ManagerStatisticsDto> GetStatisticsBySectionManager(int managerId);

}