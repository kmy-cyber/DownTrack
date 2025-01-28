using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
namespace DownTrack.Application.IServices;

public interface IEquipmentDecommissioningQueryServices : IGenericQueryService<GetEquipmentDecommissioningDto>
{
     Task<PagedResultDto<GetEquipmentDecommissioningDto>> GetEquipmentDecomissioningOfReceptorAsync(int receptorId, PagedRequestDto paged);

}