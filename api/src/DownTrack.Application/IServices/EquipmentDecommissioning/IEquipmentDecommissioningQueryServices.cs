using DownTrack.Application.DTO;
<<<<<<< HEAD
using DownTrack.Application.DTO.Paged;
=======
>>>>>>> fc2
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IEquipmentDecommissioningQueryServices : 
                                        IGenericQueryService<EquipmentDecommissioning,GetEquipmentDecommissioningDto>
{
     Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetEquipmentDecomissioningOfReceptorAsync(int receptorId, PagedRequestDto paged);

}