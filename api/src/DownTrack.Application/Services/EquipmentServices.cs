using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentServices : IEquipmentServices
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    public EquipmentServices(IEquipmentRepository equipmentRepository, IMapper mapper)
    {
        _equipmentRepository = equipmentRepository;
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

    /// <summary>
    /// Retrieves a equipment by their ID
    /// </summary>
    /// <param name="equipmentDto">The equipment's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the equipment</returns>
    public async Task<EquipmentDto> GetByIdAsync(int equipmentDto)
    {
        var result = await _equipmentRepository.GetByIdAsync(equipmentDto);

        /// and returns the updated equipment as a EquipmentDto.
        return _mapper.Map<EquipmentDto>(result);

    }
}