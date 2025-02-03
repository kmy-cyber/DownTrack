using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class TransferQueryServices : GenericQueryServices<Transfer, GetTransferDto>,
                                     ITransferQueryServices
{
    private static readonly Expression<Func<Transfer, object>>[] includes =
                            { t=> t.ShippingSupervisor!.User!,
                              t=> t.EquipmentReceptor!.User!,
                              t=> t.TransferRequest.Equipment };
    public TransferQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {
    }


    public override Expression<Func<Transfer, object>>[] GetIncludes() => includes;

    public async Task<PagedResultDto<GetTransferDto>> GetPagedTransferRequestedbyManager(int managerId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate

        IQueryable<Transfer> queryTransferRequest = _unitOfWork.GetRepository<Transfer>()
                                                                      .GetAllByItems(t => t.TransferRequest.SectionManagerId == managerId);


        return await GetPagedResultByQueryAsync(paged, queryTransferRequest);


    }

    public async Task<PagedResultDto<GetTransferDto>> GetTransferBetweenSections(PagedRequestDto paged,int sectionSource,int sectionArrival)
    {
        var transfers= _unitOfWork.GetRepository<Transfer>()
                                        .GetAllByItems(t=> t.TransferRequest.SourceDepartment!.SectionId == sectionSource,
                                                       t=> t.TransferRequest.ArrivalDepartment.SectionId == sectionArrival);

        return await GetPagedResultByQueryAsync(paged,transfers);
                                        
    }

    public async Task<PagedResultDto<GetTransferDto>> GetTransferByReceptorId(PagedRequestDto paged,int receptorId)
    {
        var queryTransfer = _unitOfWork.GetRepository<Transfer>()
                                             .GetAllByItems(t=> t.EquipmentReceptorId== receptorId);
        
        return await GetPagedResultByQueryAsync(paged,queryTransfer);
    }

}
