using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningServices : IEquipmentDecommissioningServices
{
    private readonly IEquipmentDecommissioningRepository _equipmentDecommissioningRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly ITechnicianRepository _technicianRepository;
    private readonly IMapper _mapper;

    public EquipmentDecommissioningServices(IEquipmentDecommissioningRepository equipmentDecommissioningRepository,
                                             IEquipmentRepository equipmentRepository,  
                                             ITechnicianRepository technicianRepository, 
                                             IMapper mapper)
    {
        _equipmentDecommissioningRepository = equipmentDecommissioningRepository;
        _equipmentRepository = equipmentRepository;
        _technicianRepository = technicianRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentDecommissioningDto> CreateAsync(EquipmentDecommissioningDto dto)
    {
        // Validate that all required fields are not null
        if (dto.EquipmentId == 0)
        {
            throw new ArgumentException("EquipmentId cannot be null.");
        }
        if (dto.TechnicianId == 0)
        {
            throw new ArgumentException("TechnicianId cannot be null.");
        }
        if (string.IsNullOrEmpty(dto.DecommissioningReason))
        {
            throw new ArgumentException("Reason cannot be null or empty.");
        }  
        
        // Check if the equipment exists
        var equipment = await _equipmentRepository.GetByIdAsync(dto.EquipmentId);
        if (equipment == null)
        {
            throw new KeyNotFoundException($"An equipment with '{dto.EquipmentId}' ID was not found.");
        }
        
        // Check if the equipment is already decommissioned
        if(equipment.Status == EquipmentStatus.Decommissioned)
        {
            throw new InvalidOperationException("Cannot decommission an equipment that is already decommissioned.");
        }

        // Check if the technician exists
        var technician = await _technicianRepository.GetByIdAsync(dto.TechnicianId);
        if (technician == null)
        {
            throw new KeyNotFoundException($"A technician with '{dto.TechnicianId}' ID was not found.");
        }

        var equipmentDecommissioning = _mapper.Map<EquipmentDecommissioning>(dto);
        
        // Associate the decommissioning with the equipment
        equipment.EquipmentDecommissioning = equipmentDecommissioning;

        // Add the decommissioning to the technician's decommissioning group
        technician.EquipmentDecommissionings ??= new List<EquipmentDecommissioning>();
        technician.EquipmentDecommissionings!.Add(equipmentDecommissioning);

        // update the equipment status
        equipment.Status = EquipmentStatus.Decommissioned;

        // Save changes to the database
        await _equipmentDecommissioningRepository.CreateAsync(equipmentDecommissioning);
        await _equipmentRepository.UpdateAsync(equipment);
        await _technicianRepository.UpdateAsync(technician);
        
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task DeleteAsync(int id)
    {
        var equipmentDecommissioning = await _equipmentDecommissioningRepository.GetByIdAsync(id);

        // Check if the technician and equipment are null
        if (equipmentDecommissioning.TechnicianId != null || equipmentDecommissioning.EquipmentId != null)
        {
            throw new InvalidOperationException("Cannot delete the decommissioning record because the technician or equipment is not null.");
        }

        await _equipmentDecommissioningRepository.DeleteByIdAsync(id);
    }

    public async Task<IEnumerable<EquipmentDecommissioningDto>> ListAsync()
    {
        var equipmentDecommissioning = await _equipmentDecommissioningRepository.ListAsync();
        return equipmentDecommissioning.Select(_mapper.Map<EquipmentDecommissioningDto>);
    }

    public async Task<EquipmentDecommissioningDto> UpdateAsync(EquipmentDecommissioningDto dto)
    {
        var equipmentDecommissioning = _equipmentDecommissioningRepository.GetById(dto.Id);
        _mapper.Map(dto, equipmentDecommissioning);
        await _equipmentDecommissioningRepository.UpdateAsync(equipmentDecommissioning);
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }
}
