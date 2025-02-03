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
/// Handles queries related to equipment management, including paged queries and specific equipment retrieval.
/// </summary>
public class EquipmentQueryServices : GenericQueryServices<Equipment, GetEquipmentDto>,
                                      IEquipmentQueryServices
{
    private static readonly Expression<Func<Equipment, object>>[] includes =
                            { d => d.Department,
                              d => d.Department.Section };

    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentQueryServices"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper mapper.</param>
    public EquipmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }

    /// <summary>
    /// Gets the includes expressions used for lazy loading.
    /// </summary>
    /// <returns>An array of Expression<Func<Equipment, object>> representing the includes.</returns>
    public override Expression<Func<Equipment, object>>[] GetIncludes() => includes;


    /// <summary>
    /// Retrieves paged equipment records by section manager ID asynchronously.
    /// </summary>
    /// <param name="paged">The paged request DTO.</param>
    /// <param name="sectionManagerId">The ID of the section manager.</param>
    /// <returns>A paged result DTO containing the queried equipment details.</returns>
    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionManagerIdAsync(PagedRequestDto paged,
                                                                                                int sectionManagerId)
    {

        // Validate section manager ID
        var checkSectionManager = await _unitOfWork.GetRepository<Employee>()
                                             .GetByIdAsync(sectionManagerId);

        if (checkSectionManager.UserRole != "SectionManager")
            throw new Exception($"SectionManager with Id : {sectionManagerId} not exists");


        // Query equipment related to the section manager
        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.Department.Section.SectionManagerId == sectionManagerId);

        return await GetPagedResultByQueryAsync(paged, queryEquipment);

    }

    /// <summary>
    /// Retrieves paged equipment records by section ID asynchronously.
    /// </summary>
    /// <param name="paged">The paged request DTO.</param>
    /// <returns>A paged result DTO containing the details of the equipment that belongs to a section.</returns>

    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionIdAsync(PagedRequestDto paged
                                                                                            , int sectionId)
    {
        //check section exist
        var checkSection = await _unitOfWork.GetRepository<Section>()
                                             .GetByIdAsync(sectionId);


        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.Department.SectionId == sectionId);

        return await GetPagedResultByQueryAsync(paged, queryEquipment);
    }

    /// <summary>
    /// Retrieves paged equipment records by department ID asynchronously.
    /// </summary>
    /// <param name="paged">The paged request DTO.</param>
    /// <param name="departmentId">The ID of the department.</param>
    /// <returns>A paged result DTO containing the queried equipment details.</returns>
    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByDepartmentIdAsync(PagedRequestDto paged,
                                                                                            int departmentId)
    {
        // check the department exist
        var checkDepartment = await _unitOfWork.DepartmentRepository
                                             .GetByIdAsync(departmentId);

        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.DepartmentId == departmentId);

        return await GetPagedResultByQueryAsync(paged, queryEquipment);
    }


    /// <summary>
    /// Retrieves paged equipment records by sequipment name  asynchronously.
    /// </summary>
    /// <param name="paged">The paged request DTO.</param>
    /// <param name="equipmentName">The name of the equipment.</param>
    /// <returns>A paged result DTO containing the queried equipment details.</returns>
    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByNameAsync(PagedRequestDto paged,
                                                                                     string equipmentName)
    {

        var includes = GetIncludes();

        var equipmentQuery = _unitOfWork.GetRepository<Equipment>()
                                  .GetAllByItems(e => e.Name == equipmentName);

        foreach (var include in includes)
        {
            equipmentQuery = equipmentQuery.Include(include);
        }

        return await GetPagedResultByQueryAsync(paged, equipmentQuery);
    }
    /// <summary>
    /// Retrieves paged active equipment asynchronously.
    /// </summary>
    /// <param name="paged">The paged request DTO.</param>
    /// <returns>A paged result DTO containing the queried equipment details.</returns>

    public async Task<PagedResultDto<GetEquipmentDto>> GetActiveEquipment(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Equipment> queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                                          .GetAllByItems(e => e.Status == "Active")
                                                          .Include(e => e.Department)
                                                          .Include(e => e.Department.Section);

        return await GetPagedResultByQueryAsync(paged, queryEquipment);
    }


    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByNameAndSectionManagerAsync(PagedRequestDto paged, string equipmentName, int sectionManagerId)
    {
        var includes = GetIncludes();
        var equipmentQuery = _unitOfWork.GetRepository<Equipment>()
                                        .GetAllByItems(e => e.Name == equipmentName,
                                                        e => e.Department.Section.SectionManagerId == sectionManagerId);

        return await GetPagedResultByQueryAsync(paged, equipmentQuery);
    }


    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsWith3MaintenancesAsync(PagedRequestDto paged)
    {
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);
        var includes = GetIncludes();
        var equipmentQuery = _unitOfWork.GetRepository<Equipment>()
                                        .GetAllByItems()
                                        .Where(e => e.DoneMaintenances
                                            .Count(m => m.Date >= oneYearAgo) > 3);

        return await GetPagedResultByQueryAsync(paged, equipmentQuery);
    }
}