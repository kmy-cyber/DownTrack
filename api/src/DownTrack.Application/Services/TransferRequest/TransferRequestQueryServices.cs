using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

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

    public override Expression<Func<TransferRequest, object>>[] GetIncludes()=> includes;

}




