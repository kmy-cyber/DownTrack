using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.Interfaces;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class TransferQueryServices : GenericQueryServices<Transfer,GetTransferDto>,
                                     ITransferQueryServices
{
    private static readonly Expression<Func<Transfer, object>>[] includes = 
                            { t=> t.ShippingSupervisor!.User!,
                              t=> t.EquipmentReceptor!.User! };
    public TransferQueryServices(IUnitOfWork unitOfWork, IMapper mapper,
                                 IFilterService<Transfer> filterService,
                                 ISortService<Transfer> sortService,
                                 IPaginationService<Transfer> paginationService)
        : base(unitOfWork, filterService,sortService,paginationService,mapper)
    {

    }


    public override Expression<Func<Transfer, object>>[] GetIncludes()=> includes;


}
