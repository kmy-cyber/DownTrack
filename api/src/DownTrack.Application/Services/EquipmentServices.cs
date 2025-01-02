using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentServices : IEquipmentServices
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IEquipmentDecommissioningRepository _equipmentDecommissioningRepository;
    private readonly IMapper _mapper;

    public EquipmentServices(IEquipmentRepository equipmentRepository, IEquipmentDecommissioningRepository equipmentDecommissioningRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
        _equipmentDecommissioningRepository = equipmentDecommissioningRepository;
        _mapper = mapper;
    }

    public async Task<EquipmentDto> CreateAsync(EquipmentDto dto)
    {
        var equipment = _mapper.Map<Equipment>(dto);
        await _equipmentRepository.CreateAsync(equipment);
        return _mapper.Map<EquipmentDto>(equipment);
    }

    public async Task DeleteAsync(int dto)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(dto);

        // Set EquipmentId to null in all related decommissionings
        var decommissionings = await _equipmentDecommissioningRepository.ListAsync();
        foreach (var decommissioning in decommissionings)
        {
            if (decommissioning.EquipmentId == dto)
            {
                decommissioning.EquipmentId = null;
                await _equipmentDecommissioningRepository.UpdateAsync(decommissioning);
            }
        }

        await _equipmentRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<EquipmentDto>> ListAsync()
    {
        var equipment = await _equipmentRepository.ListAsync();
        return equipment.Select(_mapper.Map<EquipmentDto>);
    }

    public async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        var equipment = _equipmentRepository.GetById(dto.Id);
        _mapper.Map(dto, equipment);
        await _equipmentRepository.UpdateAsync(equipment);
        return _mapper.Map<EquipmentDto>(equipment);
    }
}