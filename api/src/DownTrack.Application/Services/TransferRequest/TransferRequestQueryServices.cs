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
                              tr=> tr.ArrivalDepartment.Section,
                              tr=> tr.Equipment.Department,
                              tr=> tr.Equipment.Department.Section };
    public TransferRequestQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base (unitOfWork, mapper)
    {

    }
 
    public override Expression<Func<TransferRequest, object>>[] GetIncludes()=> includes;
    
    public async Task<PagedResultDto<GetTransferRequestDto>> GetPagedRequestsofArrivalDepartmentAsync(int receptorId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        
        var arrivalDepartment =  _unitOfWork.GetRepository<EquipmentReceptor>().GetById(receptorId).DepartmentId;


        IQueryable<TransferRequest> queryTransferRequest = _unitOfWork.GetRepository<TransferRequest>()
                                                                      .GetAllByItems(tr=> tr.ArrivalDepartmentId == arrivalDepartment);

        
        return await GetPagedResultByQueryAsync(paged,queryTransferRequest);


    }

    

}




