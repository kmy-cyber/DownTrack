using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IEquipmentDecommissioningQueryServices : 
                                        IGenericQueryService<EquipmentDecommissioning,GetEquipmentDecommissioningDto>
{
    
}