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
/// Handles the business logic related to employee and works with DTOs to interact with clients,
/// using the repository interface to access the database.
/// </summary>

public class EmployeeQueryServices : GenericQueryServices<Employee, GetEmployeeDto>,
                                     IEmployeeQueryServices
{

    private static readonly Expression<Func<Employee, object>>[] includes =
                            { e => e.User! };

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeQueryService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper mapper.</param>
    public EmployeeQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }


    /// <summary>
    /// Gets the includes expressions used for lazy loading.
    /// </summary>
    /// <returns>An array of Expression<Func<Employee, object>> representing the includes.</returns>
    public override Expression<Func<Employee, object>>[] GetIncludes() => includes;


    /// <summary>
    /// Lists all employees by role asynchronously.
    /// </summary>
    /// <param name="role">The user role to filter by.</param>
    /// <returns>A collection of GetEmployeeDto objects representing the filtered employees.</returns>
    public async Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role)
    {

        var rolesQuery = _unitOfWork.GetRepository<Employee>()
                                      .GetAllByItems(u => u.UserRole == role.ToString());

        // Apply includes if any
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

    /// <summary>
    /// Gets an employee by username asynchronously.
    /// </summary>
    /// <param name="employeeUserName">The username of the employee to retrieve.</param>
    /// <returns>A GetEmployeeDto object representing the employee, or null if not found.</returns>
    public async Task<GetEmployeeDto> GetByUserNameAsync(string employeeUserName)
    {

        var expressions = new Expression<Func<Employee, bool>>[]
        {
            e=> e.UserName == employeeUserName
        };

        // Apply includes if any
        var includes = GetIncludes();

        var employee = await _unitOfWork.GetRepository<Employee>()
                                 .GetByItems(expressions, includes);


        if (employee == null)
            throw new Exception($"No employee found with the username '{employeeUserName}'.");

        return _mapper.Map<GetEmployeeDto>(employee);

    }



}
