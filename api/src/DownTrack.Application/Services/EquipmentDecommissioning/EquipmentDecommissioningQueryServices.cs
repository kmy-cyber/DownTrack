using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningQueryServices : IEquipmentDecommissioningQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public EquipmentDecommissioningQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetEquipmentDecommissioningDto> GetByIdAsync(int dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                        .GetByIdAsync(dto, default,
                                                                     ed => ed.Technician!.User!,
                                                                     ed => ed.Equipment!.Department,
                                                                     ed => ed.Equipment!.Department.Section,
                                                                     ed => ed.Receptor!.User!);

        return _mapper.Map<GetEquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task<IEnumerable<GetEquipmentDecommissioningDto>> ListAsync()
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                        .GetAll()
                                                        .Include(ed => ed.Technician!.User!)
                                                        .Include(ed => ed.Equipment!.Department!)
                                                        .Include(ed => ed.Equipment!.Department!.Section!)
                                                        .Include(ed => ed.Receptor!.User!)
                                                        .ToListAsync();

        return equipmentDecommissioning.Select(_mapper.Map<GetEquipmentDecommissioningDto>);
    }

    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<EquipmentDecommissioning> queryEquipmentDecommissioning = _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                                                        .GetAll()
                                                        .Include(ed => ed.Technician!.User!)
                                                        .Include(ed => ed.Equipment!.Department!)
                                                        .Include(ed => ed.Equipment!.Department!.Section!)
                                                        .Include(ed => ed.Receptor!.User!);

        var totalCount = await queryEquipmentDecommissioning.CountAsync();


        var items = await queryEquipmentDecommissioning // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetEquipmentDecommissioningDto>
        {
            Items = items?.Select(_mapper.Map<GetEquipmentDecommissioningDto>) ?? Enumerable.Empty<GetEquipmentDecommissioningDto>(),
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


    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetEquipmentDecomissioningOfReceptorAsync(int receptorId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<EquipmentDecommissioning> queryEquipmentDecommissioning =  _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                                                        .GetAllByItems(ed=> ed.ReceptorId == receptorId && ed.Status=="Pending")
                                                                                        .Include(ed => ed.Technician!.User!)
                                                                                        .Include(ed => ed.Equipment!.Department!)
                                                                                        .Include(ed => ed.Equipment!.Department!.Section!)
                                                                                        .Include(ed => ed.Receptor!.User!);

        var totalCount = await queryEquipmentDecommissioning.CountAsync();


        var items = await queryEquipmentDecommissioning // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetEquipmentDecommissioningDto>
        {
            Items = items?.Select(_mapper.Map<GetEquipmentDecommissioningDto>) ?? Enumerable.Empty<GetEquipmentDecommissioningDto>(),
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