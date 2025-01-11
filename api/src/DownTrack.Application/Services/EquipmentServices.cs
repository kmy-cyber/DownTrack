using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentServices : IEquipmentServices
{
    //private readonly IEquipmentRepository _equipmentRepository;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EquipmentServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _equipmentRepository = equipmentRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentDto> CreateAsync(EquipmentDto dto)
    {
        var equipment = _mapper.Map<Equipment>(dto);

        //await _equipmentRepository.CreateAsync(equipment);
        
        await _unitOfWork.GetRepository<Equipment>().CreateAsync(equipment);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDto>(equipment);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Equipment>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
        //await _equipmentRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<EquipmentDto>> ListAsync()
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetAllAsync().ToListAsync();
        //var equipment = await _equipmentRepository.ListAsync();
        return equipment.Select(_mapper.Map<EquipmentDto>);
    }

    public async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(dto.Id);

        //var equipment = _equipmentRepository.GetById(dto.Id);
        _mapper.Map(dto, equipment);

        _unitOfWork.GetRepository<Equipment>().Update(equipment);

        await _unitOfWork.CompleteAsync();
        //await _equipmentRepository.UpdateAsync(equipment);
        return _mapper.Map<EquipmentDto>(equipment);
    }

    /// <summary>
    /// Retrieves an equipment by their ID
    /// </summary>
    /// <param name="equipmentDto">The equipment's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the equipment</returns>
    public async Task<EquipmentDto> GetByIdAsync(int equipmentDto)
    {
        var result = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDto);
        
        //var result = await _equipmentRepository.GetByIdAsync(equipmentDto);

        /// and returns the updated equipment as an EquipmentDto.
        return _mapper.Map<EquipmentDto>(result);

    }
}