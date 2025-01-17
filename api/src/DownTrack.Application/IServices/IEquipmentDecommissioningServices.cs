using DownTrack.Application.DTO;

namespace DownTrack.Application.IServices;

public interface IEquipmentDecommissioningServices : IGenericService<EquipmentDecommissioningDto>
{
    Task AcceptDecommissioningAsync(int equipmentDecommissioningId);
}