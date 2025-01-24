

using DownTrack.Application.DTO;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Const;


public static class GetDto 
{
    static Dictionary< Type , Type> dtoMapping  = new Dictionary< Type , Type>
    {
        {typeof(EmployeeDto), typeof(Employee)},
        {typeof(TechnicianDto), typeof(Technician)},
        {typeof(EquipmentReceptorDto), typeof(EquipmentReceptor)},
        {typeof(EquipmentDto), typeof(Equipment)},
        {typeof(DoneMaintenanceDto), typeof(DoneMaintenance)},
        {typeof(EquipmentReceptorDto), typeof(EquipmentReceptor)},
        
    };

    public static Type? GetEntityTypeForDto(Type dto)
    {

        return dtoMapping.ContainsKey(dto) ? dtoMapping[dto] : null;
    }

    
}