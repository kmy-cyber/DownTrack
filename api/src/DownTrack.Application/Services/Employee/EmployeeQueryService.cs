using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using AutoMapper;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Roles;
using System.Linq.Expressions;
using DownTrack.Application.DTO.Statistics;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to employee and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class EmployeeQueryServices : GenericQueryServices<Employee, GetEmployeeDto>,
                                     IEmployeeQueryServices
{

    private static readonly Expression<Func<Employee, object>>[] includes =
                            { e => e.User! };

    public EmployeeQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }


    public override Expression<Func<Employee, object>>[] GetIncludes() => includes;

    public async Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role)
    {

        var rolesQuery = _unitOfWork.GetRepository<Employee>()
                                      .GetAllByItems(u => u.UserRole == role.ToString());
        var includes = GetIncludes();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                rolesQuery = rolesQuery.Include(exp); // Apply the filter expression to the query.
            }
        }
        var rolesList = await rolesQuery.ToListAsync();

        return rolesList.Select(_mapper.Map<GetEmployeeDto>);

    }

    public async Task<GetEmployeeDto> GetByUserNameAsync(string employeeUserName)
    {

        var expressions = new Expression<Func<Employee, bool>>[]
        {
            e=> e.UserName == employeeUserName
        };

        var includes = GetIncludes();

        var employee = await _unitOfWork.GetRepository<Employee>()
                                 .GetByItems(expressions, includes);


        if (employee == null)
            throw new Exception($"No employee found with the username '{employeeUserName}'.");

        return _mapper.Map<GetEmployeeDto>(employee);

    }


    
}
