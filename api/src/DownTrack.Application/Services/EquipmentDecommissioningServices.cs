using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningServices : IEquipmentDecommissioningServices
{
    private readonly IEquipmentDecommissioningRepository _equipmentDecommissioningRepository;
    private readonly IMapper _mapper;
    
    public EquipmentDecommissioningServices(IEquipmentDecommissioningRepository equipmentDecommissioningRepository, IMapper mapper)
    {
        _equipmentDecommissioningRepository = equipmentDecommissioningRepository;
        _mapper = mapper;
    }    
    
    public async Task<EquipmentDecommissioningDto> CreateAsync(EquipmentDecommissioningDto dto)
    {
        var equipmentDecommissioning = _mapper.Map<EquipmentDecommissioning>(dto);
        await _equipmentDecommissioningRepository.CreateAsync(equipmentDecommissioning);
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task DeleteAsync(int dto)
    {
        await _equipmentDecommissioningRepository.DeleteByIdAsync(dto);
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
