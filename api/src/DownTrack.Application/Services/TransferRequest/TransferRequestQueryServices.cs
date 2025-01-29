using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TransferRequestQueryServices :GenericQueryServices<TransferRequest,GetTransferRequestDto>,
                                           ITransferRequestQueryServices
{
    private static readonly Expression<Func<TransferRequest, object>>[] includes = 
                            { tr => tr.SectionManager!.User!,
                              tr=> tr.ArrivalDepartment,
                              tr=> tr.Equipment,
                              tr=> tr.ArrivalDepartment.Section };
    public TransferRequestQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base (unitOfWork, mapper)
    {
    }

 
    

    public async Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int receptorId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        
        var arrivalDepartment =  _unitOfWork.GetRepository<EquipmentReceptor>().GetById(receptorId).DepartmentId;


        IQueryable<TransferRequest> queryTransferRequest = _unitOfWork.GetRepository<TransferRequest>()
                                                                      .GetAllByItems(tr=> tr.ArrivalDepartmentId == arrivalDepartment && tr.Status == "Unregistered")
                                                                      .Include(tr => tr.SectionManager!.User)
                                                                      .Include(tr=> tr.ArrivalDepartment)
                                                                      .Include(tr=> tr.Equipment)                                                                  
                                                                      .ThenInclude(e=> e.Department)
                                                                      .ThenInclude(d=> d.Section);

        var totalCount = await queryTransferRequest.CountAsync();

        var items = await queryTransferRequest // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetTransferRequestDto>
        {
            Items = items?.Select(_mapper.Map<GetTransferRequestDto>) ?? Enumerable.Empty<GetTransferRequestDto>(),
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



    

    public override Expression<Func<TransferRequest, object>>[] GetIncludes()=> includes;

}




