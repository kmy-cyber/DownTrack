using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.Interfaces;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningQueryServices :GenericQueryServices<EquipmentDecommissioning,GetEquipmentDecommissioningDto>,
                                                    IEquipmentDecommissioningQueryServices
{
    private static readonly Expression<Func<EquipmentDecommissioning, object>>[] includes = 
                            { ed=> ed.Technician!.User!,
                              ed=> ed.Equipment!,
                              ed=> ed.Receptor!.User! };

    public EquipmentDecommissioningQueryServices(IUnitOfWork unitOfWork, IMapper mapper,
                                 IFilterService<EquipmentDecommissioning> filterService,
                                 ISortService<EquipmentDecommissioning> sortService,
                                 IPaginationService<EquipmentDecommissioning> paginationService)
        : base(unitOfWork, filterService,sortService,paginationService,mapper)
    {

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




    public override Expression<Func<EquipmentDecommissioning, object>>[] GetIncludes()=> includes;

 
}