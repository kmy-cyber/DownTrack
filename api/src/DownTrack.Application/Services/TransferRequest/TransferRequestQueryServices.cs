using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.Interfaces;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;


namespace DownTrack.Application.Services;

public class TransferRequestQueryServices :GenericQueryServices<TransferRequest,GetTransferRequestDto>,
                                           ITransferRequestQueryServices
{
    private static readonly Expression<Func<TransferRequest, object>>[] includes = 
                            { tr => tr.SectionManager!.User!,
                              tr=> tr.ArrivalDepartment,
                              tr=> tr.SourceDepartment!,
                              tr=> tr.Equipment,
                              tr=> tr.ArrivalDepartment.Section ,
                              tr=> tr.SourceDepartment!.Section};
                              
    public TransferRequestQueryServices(IUnitOfWork unitOfWork, IMapper mapper,
                                 IFilterService<TransferRequest> filterService,
                                 ISortService<TransferRequest> sortService,
                                 IPaginationService<TransferRequest> paginationService)
        : base(unitOfWork, filterService,sortService,paginationService,mapper)
    {

    }

    public override Expression<Func<TransferRequest, object>>[] GetIncludes()=> includes;
    

    public async Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int receptorId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        
        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(receptorId);


        IQueryable<TransferRequest> queryTransferRequest = _unitOfWork.GetRepository<TransferRequest>()
                                                                      .GetAllByItems(tr=> tr.ArrivalDepartmentId == receptor.DepartmentId,
                                                                                     tr=> tr.Equipment.Status == TransferRequestStatus.Pending.ToString());

        return await GetPagedResultByQueryAsync(paged,queryTransferRequest);
    }


}




