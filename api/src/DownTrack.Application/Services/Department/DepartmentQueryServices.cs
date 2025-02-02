
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
public class DepartmentQueryServices : GenericQueryServices<Department,GetDepartmentDto>,
                                       IDepartmentQueryServices
{
    private static readonly Expression<Func<Department, object>>[] includes = 
                            { d => d.Section };
    public DepartmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }

    public override Expression<Func<Department, object>>[] GetIncludes()=> includes;


    public async Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsInSection (int sectionId)
    {
        var section =await _unitOfWork.GetRepository<Section>().GetByIdAsync(sectionId);
        
        var departmentsQuery = _unitOfWork.GetRepository<Department>()
                                        .GetAllByItems(d=> d.SectionId == sectionId);

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


    public async Task<PagedResultDto<GetDepartmentDto>> GetPagedAllDepartmentsInSection(PagedRequestDto paged, int sectionId)
    {
        //check section exist

        var section =await _unitOfWork.GetRepository<Section>().GetByIdAsync(sectionId);
        
        var departmentsQuery = _unitOfWork.GetRepository<Department>()
                                        .GetAllByItems(d=> d.SectionId == sectionId);

        return await GetPagedResultByQueryAsync(paged,departmentsQuery);
    }

}