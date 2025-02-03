using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentQueryServices : GenericQueryServices<Equipment, GetEquipmentDto>,
                                      IEquipmentQueryServices
{
    private static readonly Expression<Func<Equipment, object>>[] includes =
                            { d => d.Department,
                              d => d.Department.Section };
    public EquipmentQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }

    public override Expression<Func<Equipment, object>>[] GetIncludes() => includes;


    public async Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionManagerIdAsync(PagedRequestDto paged,
                                                                                                int sectionManagerId)
    {
        //check sectionManagerId es valid

        var checkSectionManager = await _unitOfWork.GetRepository<Employee>()
                                             .GetByIdAsync(sectionManagerId);

        if (checkSectionManager.UserRole != "SectionManager")
            throw new Exception($"SectionManager with Id : {sectionManagerId} not exists");


        var queryEquipment = _unitOfWork.GetRepository<Equipment>()
                                         .GetAllByItems(e => e.Department.Section.SectionManagerId == sectionManagerId);

        return await GetPagedResultByQueryAsync(paged, queryEquipment);

    }

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

    public async Task<PagedResultDto<GetEquipmentDto>> GetTransferredEquipmentsByDepartmentAsync(PagedRequestDto paged, int departmentId)
    {
        var query = _unitOfWork.GetRepository<Transfer>()
                                .GetAllByItems(t => t.TransferRequest.ArrivalDepartmentId == departmentId)
                                .Include(t => t.TransferRequest)
                                    .ThenInclude(tr => tr.Equipment)
                                        .ThenInclude(e => e.Department)
                                            .ThenInclude(d => d.Section);

        var totalCount = await query.CountAsync();

        var items = await query
                            .Skip((paged.PageNumber - 1) * paged.PageSize)
                            .Take(paged.PageSize)
                            .Select(t => new GetEquipmentDto
                            {
                                Id = t.TransferRequest.Equipment.Id,
                                Name = t.TransferRequest.Equipment.Name,
                                Type = t.TransferRequest.Equipment.Type,
                                Status = t.TransferRequest.Equipment.Status,
                                DateOfadquisition = t.TransferRequest.Equipment.DateOfadquisition,
                                DepartmentId = t.TransferRequest.Equipment.DepartmentId,
                                SectionId = t.TransferRequest.Equipment.Department.SectionId , // Evitar null
                                DepartmentName = t.TransferRequest.Equipment.Department.Name,
                                SectionName = t.TransferRequest.Equipment.Department.Section.Name
                            })
                            .ToListAsync();

        return new PagedResultDto<GetEquipmentDto>
        {
            Items = items,
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

}