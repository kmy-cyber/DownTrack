using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningQueryServices :GenericQueryServices<EquipmentDecommissioning,GetEquipmentDecommissioningDto>,
                                                    IEquipmentDecommissioningQueryServices
{
    private static readonly Expression<Func<EquipmentDecommissioning, object>>[] includes = 
                            { ed=> ed.Technician!.User!,
                              ed=> ed.Equipment!,
                              ed=> ed.Receptor!.User! };

    public EquipmentDecommissioningQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
        :base (unitOfWork, mapper)
    {

    }

    public override Expression<Func<EquipmentDecommissioning, object>>[] GetIncludes()=> includes;

 
}