
using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.IServices;

public interface IEmployeeQueryServices : IGenericQueryService<Employee,GetEmployeeDto>
{
    Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role);

}