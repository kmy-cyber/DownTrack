using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;

public interface IEquipmentDecommissioningCommandServices : IGenericCommandService<EquipmentDecommissioningDto>
{
    Task AcceptDecommissioningAsync(int equipmentDecommissioningId);

}