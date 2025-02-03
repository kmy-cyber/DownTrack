
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
/// This service class handles queries related to departments within the application.
/// It provides methods for retrieving department data, including filtering by section,
/// pagination support, and mapping results to DTOs for client communication.
/// The class interacts with repositories and utilizes unit of work pattern for database operations.
/// </summary>


public class DepartmentQueryServices : GenericQueryServices<Department, GetDepartmentDto>,
                                       IDepartmentQueryServices
{
    private static readonly Expression<Func<Department, object>>[] includes =
                            { d => d.Section };
    public DepartmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }
    /// <summary>
    /// Gets the list of expressions used for including related entities in queries.
    /// </summary>
    /// <returns>An array of expressions representing included entities.</returns>

    public override Expression<Func<Department, object>>[] GetIncludes() => includes;

    /// <summary>
    /// Retrieves all departments within a specific section.
    /// </summary>
    /// <param name="sectionId">The ID of the section to filter departments by.</param>
    /// <returns>A task containing a collection of GetDepartmentDto objects representing departments in the specified section.</returns>

    public async Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsInSection(int sectionId)
    {
        var section = await _unitOfWork.GetRepository<Section>().GetByIdAsync(sectionId);

        var departmentsQuery = _unitOfWork.GetRepository<Department>()
                                        .GetAllByItems(d => d.SectionId == sectionId);

        var includes = GetIncludes();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                departmentsQuery = departmentsQuery.Include(exp); // Apply the filter expression to the query.
            }
        }

        var departmentsList = await departmentsQuery.ToListAsync();

        return departmentsList.Select(_mapper.Map<GetDepartmentDto>);
    }

    /// <summary>
    /// Retrieves paginated results of all departments within a specific section.
    /// </summary>
    /// <param name="paged">A PagedRequestDto object specifying pagination parameters.</param>
    /// <param name="sectionId">The ID of the section to filter departments by.</param>
    /// <returns>A PagedResultDto<GetDepartmentDto> containing paginated results of departments in the specified section.</returns>

    public async Task<PagedResultDto<GetDepartmentDto>> GetPagedAllDepartmentsInSection(PagedRequestDto paged, int sectionId)
    {
        //check section exist

        var section = await _unitOfWork.GetRepository<Section>().GetByIdAsync(sectionId);

        var departmentsQuery = _unitOfWork.GetRepository<Department>()
                                        .GetAllByItems(d => d.SectionId == sectionId);

        return await GetPagedResultByQueryAsync(paged, departmentsQuery);
    }

}