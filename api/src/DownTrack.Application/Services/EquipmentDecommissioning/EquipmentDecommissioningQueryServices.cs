using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningQueryServices :GenericQueryServices<EquipmentDecommissioning,GetEquipmentDecommissioningDto>,
                                                    IEquipmentDecommissioningQueryServices
{
    private static readonly Expression<Func<EquipmentDecommissioning, object>>[] includes = 
                            { ed=> ed.Technician!.User!,
                              ed=> ed.Equipment!.Department,
                              ed=> ed.Receptor!.User!,
                              ed=> ed.Equipment!.Department.Section };

    public EquipmentDecommissioningQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        :base (unitOfWork, mapper)
    {

    }

    public override Expression<Func<EquipmentDecommissioning, object>>[] GetIncludes()=> includes;


    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetEquipmentDecomissioningOfReceptorAsync(int receptorId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<EquipmentDecommissioning> queryEquipmentDecommissioning =  _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                                                        .GetAllByItems(ed=> ed.ReceptorId == receptorId,
                                                                                                       ed=> ed.Status=="Pending");
        return await GetPagedResultByQueryAsync(paged,queryEquipmentDecommissioning);
    }




    
}