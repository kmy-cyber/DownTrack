using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningServices : IEquipmentDecommissioningServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public EquipmentDecommissioningServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentDecommissioningDto> CreateAsync(EquipmentDecommissioningDto dto)
    {
        if (dto.TechnicianId == null || dto.EquipmentId == null || dto.ReceptorId == null || string.IsNullOrEmpty(dto.Cause))
        {
            throw new ArgumentException("All fields must be non-null when creating a decommissioning.");
        }

        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(dto.EquipmentId);
        if (equipment.EquipmentStatus == Domain.Enum.EquipmentStatus.Decommissioned)
        {
            throw new InvalidOperationException("The equipment is already decommissioned.");
        }

        var equipmentDecommissioning = _mapper.Map<EquipmentDecommissioning>(dto);
        equipment.EquipmentDecommissionings!.Add(equipmentDecommissioning);

        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(dto.TechnicianId);
        await _unitOfWork.GetRepository<EquipmentDecommissioning>().CreateAsync(equipmentDecommissioning);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task DeleteAsync(int dto)
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(dto);
        if (equipment != null)
        {
            throw new InvalidOperationException("The equipment associated with this decommissioning record must be null to delete the decommissioning.");
        }

        await _unitOfWork.GetRepository<EquipmentDecommissioning>().DeleteByIdAsync(dto);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<EquipmentDecommissioningDto> GetByIdAsync(int dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto);
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task<IEnumerable<EquipmentDecommissioningDto>> ListAsync()
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetAllAsync().ToListAsync();
        return equipmentDecommissioning.Select(_mapper.Map<EquipmentDecommissioningDto>);
    }

    public async Task<EquipmentDecommissioningDto> UpdateAsync(EquipmentDecommissioningDto dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto.Id);
        _mapper.Map(dto, equipmentDecommissioning);
        _unitOfWork.GetRepository<EquipmentDecommissioning>().Update(equipmentDecommissioning);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }
    public async Task AcceptDecommissioningAsync(int dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto);
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDecommissioning.EquipmentId);
        
        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(equipmentDecommissioning.ReceptorId);
        receptor.EquipmentDecommissionings.Add(equipmentDecommissioning);
        
        equipment.EquipmentStatus = Domain.Enum.EquipmentStatus.Decommissioned;
        equipment.EquipmentDecommissionings!.Add(equipmentDecommissioning);

        if (equipmentDecommissioning.TechnicianId != null)
        {
            var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(equipmentDecommissioning.TechnicianId);
            if (technician != null)
            {
                technician.EquipmentDecommissionings.Add(equipmentDecommissioning);
            }
        }

        var allDecommissionings = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetAllAsync().ToListAsync();
        foreach (var decommissioning in allDecommissionings)
        {
            if (decommissioning.EquipmentId == equipmentDecommissioning.EquipmentId && decommissioning.Id != equipmentDecommissioning.Id)
            {
                await _unitOfWork.GetRepository<EquipmentDecommissioning>().DeleteByIdAsync(decommissioning.Id);
            }
        }
        equipmentDecommissioning.Status = Domain.Enum.DecommissioningStatus.Accepted;

        _unitOfWork.GetRepository<Equipment>().Update(equipment);
        await _unitOfWork.CompleteAsync();
    }
}