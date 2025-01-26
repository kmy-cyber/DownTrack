
using DownTrack.Application.DTO;
using DownTrack.Domain.Roles;

namespace DownTrack.Application.IServices;

public interface IEmployeeQueryServices : IGenericQueryService<GetEmployeeDto>
{
    Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role);

}