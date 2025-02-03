using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

public interface IEquipmentDecommissioningQueryServices :
                                        IGenericQueryService<EquipmentDecommissioning, GetEquipmentDecommissioningDto>
{
     Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetEquipmentDecomissioningOfReceptorAsync(int receptorId, PagedRequestDto paged);

     Task<GetEquipmentDecommissioningDto> GetDecomissionByEquipmentIdAsync(int equipmentId);
     Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetAcceptedDecommissioning(PagedRequestDto paged);
     Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetDecomissionLastYear(PagedRequestDto paged);
}