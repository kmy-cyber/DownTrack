using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningQueryServices : GenericQueryServices<EquipmentDecommissioning, GetEquipmentDecommissioningDto>,
                                                    IEquipmentDecommissioningQueryServices
{
    private static readonly Expression<Func<EquipmentDecommissioning, object>>[] includes =
                            { ed=> ed.Technician!.User!,
                              ed=> ed.Equipment!,
                              ed=> ed.Receptor!.User!,
                              ed=> ed.Equipment!.Department,
                              ed=> ed.Equipment!.Department.Section };

    public EquipmentDecommissioningQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper)
    {

    }


    public override Expression<Func<EquipmentDecommissioning, object>>[] GetIncludes() => includes;


    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetEquipmentDecomissioningOfReceptorAsync(int receptorId, PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<EquipmentDecommissioning> queryEquipmentDecommissioning = _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                                                                        .GetAllByItems(ed => ed.ReceptorId == receptorId && ed.Status == "Pending")
                                                                                        .Include(ed => ed.Technician!.User!)
                                                                                        .Include(ed => ed.Equipment!.Department!)
                                                                                        .Include(ed => ed.Equipment!.Department!.Section!)
                                                                                        .Include(ed => ed.Receptor!.User!);

        return await GetPagedResultByQueryAsync(paged, queryEquipmentDecommissioning);
    }




    public async Task<GetEquipmentDecommissioningDto> GetDecomissionByEquipmentIdAsync(int equipmentId)
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentId);

        var includes = GetIncludes();

        var decommission = await _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                    .GetByItems(new Expression<Func<EquipmentDecommissioning, bool>>[]
                                    {
                                        ed=> ed.EquipmentId == equipmentId,
                                        ed=> ed.Status == DecommissioningStatus.Accepted.ToString()

                                    }, includes);

        if (decommission == null)
            throw new Exception("The equipment has not been decommissioned yet.");

        return _mapper.Map<GetEquipmentDecommissioningDto>(decommission);
    }


    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetAcceptedDecommissioning(PagedRequestDto paged)
    {
        var decomissioning = _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                   .GetAllByItems(ed => ed.Status == DecommissioningStatus.Accepted.ToString());
        return await GetPagedResultByQueryAsync(paged, decomissioning);

    }

    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetDecomissionLastYear(PagedRequestDto paged)
    {
        var decomissions = _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                            .GetAllByItems(ed=> ed.Date >= DateTime.Now.AddYears(-1),
                                                        ed=> ed.Status == DecommissioningStatus.Accepted.ToString());
        
        return await GetPagedResultByQueryAsync(paged,decomissions);
    }

    public async Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetDecomissionByReceptorAsync(PagedRequestDto paged, int receptorId)
    {
        var decomissions = _unitOfWork.GetRepository<EquipmentDecommissioning>()
                                      .GetAllByItems(ed=> ed.ReceptorId == receptorId,
                                                     ed=> ed.Status == DecommissioningStatus.Accepted.ToString());
        
        return await GetPagedResultByQueryAsync(paged,decomissions);
    }
}