
using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;

public interface IEmployeeServices : IGenericService<EmployeeDto>
{
    


    Task<IEnumerable<GetEmployeeDto>> AllAsync();
}