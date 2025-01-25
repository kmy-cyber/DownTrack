
using DownTrack.Application.DTO;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.IServices;

public interface IEmployeeServices : IGenericService<EmployeeDto>
{
    Task<IEnumerable<GetEmployeeDto>> AllAsync();
    Task<IEnumerable<EmployeeDto>> ListAllByRole(UserRole role);
}