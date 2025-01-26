using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using AutoMapper;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Roles;
using DownTrack.Application.DTO.Paged;
using System.Linq.Expressions;

namespace DownTrack.Application.Services;

/// <summary>
/// Handle the business logic related to employee and work with DTOs 
/// to interact with the client , using the repository interface to access
/// the database 
/// </summary> 
public class EmployeeQueryServices : IEmployeeQueryServices
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EmployeeQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    /// <summary>
    /// Retrieves a list of all employees.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, with a list of EmployeeDto as the result.</returns>
    public async Task<IEnumerable<GetEmployeeDto>> ListAsync()
    {
        var employee = await _unitOfWork.GetRepository<Employee>()
                                        .GetAll()
                                        .Include(e=> e.User)
                                        .ToListAsync();
        
        return employee.Select(_mapper.Map<GetEmployeeDto>);
    }

    public async Task<GetEmployeeDto> GetByIdAsync(int employeeDto)
    {
        
        var result = await _unitOfWork.GetRepository<Employee>()
                                      .GetByIdAsync(employeeDto,default, e => e.User!);

        return _mapper.Map<GetEmployeeDto>(result);

    }


    public async Task<PagedResultDto<GetEmployeeDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Employee> queryEmployee = _unitOfWork.GetRepository<Employee>()
                                                        .GetAll()
                                                        .Include(e=> e.User);

        var totalCount = await queryEmployee.CountAsync();

        var items = await queryEmployee // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.

        
        return new PagedResultDto<GetEmployeeDto>
        {
            Items = items?.Select(_mapper.Map<GetEmployeeDto>) ?? Enumerable.Empty<GetEmployeeDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }


    public async Task<IEnumerable<GetEmployeeDto>> ListAllByRole(UserRole role)
    {

        var result = await _unitOfWork.GetRepository<Employee>()
                                      .GetAllByItems(u=>u.UserRole == role.ToString())
                                      .Include(e=> e.User)
                                      .ToListAsync();

        return result.Select(_mapper.Map<GetEmployeeDto>);
        
    }


}
