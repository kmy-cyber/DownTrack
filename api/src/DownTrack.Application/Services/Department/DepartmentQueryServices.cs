
using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;


/// <summary>
/// Service class for handling business logic related to departments.
/// Interacts with repositories and uses DTOs for client communication.
/// </summary>
public class DepartmentQueryServices : IDepartmentQueryServices
{

    // Automapper instance for mapping between domain entities and DTOs.
    private readonly IMapper _mapper;

    // Unit of Work instance for managing repositories and transactions.
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    public async Task<IEnumerable<GetDepartmentDto>> ListAsync()
    {
        var departments = await _unitOfWork
                                .GetRepository<Department>()
                                .GetAll() 
                                .Include(d => d.Section) // Load the relation Section
                                .ToListAsync(); // List the values

        return departments.Select(_mapper.Map<GetDepartmentDto>);
    }




    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="departmentDto">The ID of the department to retrieve.</param>
    /// <returns>The DepartmentDto of the retrieved department.</returns>
    public async Task<GetDepartmentDto> GetByIdAsync(int departmentDto)
    {

        var result = await _unitOfWork.GetRepository<Department>()
                                      .GetByIdAsync(departmentDto,default,
                                      d=>d.Section);

        return _mapper.Map<GetDepartmentDto>(result);

    }

     public async Task<PagedResultDto<GetDepartmentDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Department> queryDepartment = _unitOfWork.GetRepository<Department>()
                                                            .GetAll()
                                                            .Include(d=>d.Section);

        var totalCount = await queryDepartment.CountAsync();

        var items = await queryDepartment // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetDepartmentDto>
        {
            Items = items?.Select(_mapper.Map<GetDepartmentDto>) ?? Enumerable.Empty<GetDepartmentDto>(),
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

    public async Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsInSection (int sectionId)
    {
        var sections = await _unitOfWork.GetRepository<Department>()
                                        .GetAllByItems(d=> d.SectionId == sectionId)
                                        .Include(d=> d.Section)
                                        .ToListAsync();

        return sections.Select(_mapper.Map<GetDepartmentDto>);
    }


}