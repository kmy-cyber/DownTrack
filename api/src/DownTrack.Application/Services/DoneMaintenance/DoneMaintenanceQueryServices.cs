using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class DoneMaintenanceQueryServices : GenericQueryServices<DoneMaintenance,GetDoneMaintenanceDto>,
                                            IDoneMaintenanceQueryServices
{
     private static readonly Expression<Func<DoneMaintenance, object>>[] includes = 
                            { dm=> dm.Technician!.User!,
                              dm=> dm.Equipment! };

    public DoneMaintenanceQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
    {

    }

    public override Expression<Func<DoneMaintenance, object>>[] GetIncludes()=> includes;

    public async Task<PagedResultDto<GetDoneMaintenanceDto>> GetByTechnicianIdAsync (PagedRequestDto paged, int technicianId)
    {
        var technician = await _unitOfWork.GetRepository<Technician>()
                                          .GetByIdAsync(technicianId);
        
        if(technician == null)
            throw new Exception($"The techncian with ID {technicianId} was not found.");

        IQueryable<DoneMaintenance> queryMaintenancesByTechnician = _unitOfWork.GetRepository<DoneMaintenance>()
                                                                                .GetAllByItems(dm=> dm.TechnicianId ==technicianId);

        return await GetPagedResultByQueryAsync(paged,queryMaintenancesByTechnician);

    }

    public async Task<PagedResultDto<GetDoneMaintenanceDto>> GetMaintenanceByTechnicianStatusAsync(PagedRequestDto paged,int technicianId, bool status)
    {
        var maintenance =  _unitOfWork.GetRepository<DoneMaintenance>()
                                           .GetAllByItems(dm=> dm.TechnicianId == technicianId,
                                                          dm=> dm.Finish == status);
        
        return await GetPagedResultByQueryAsync(paged,maintenance);
    }
}