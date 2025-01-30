using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;
using DownTrack.Domain.Status;

namespace DownTrack.Application.Services;

public class EquipmentDecommissioningCommandServices : IEquipmentDecommissioningCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public EquipmentDecommissioningCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentDecommissioningDto> CreateAsync(EquipmentDecommissioningDto dto)
    {
        var equipmentDecommissioning = _mapper.Map<EquipmentDecommissioning>(dto);

        var equipment = await _unitOfWork.GetRepository<Equipment>()
                                         .GetByIdAsync(equipmentDecommissioning.EquipmentId);

        if (equipment.Status == EquipmentStatus.Inactive.ToString())
        {
            throw new InvalidOperationException("The equipment is already inactive.");
        }

        var technician = await _unitOfWork.GetRepository<Technician>()
                                          .GetByIdAsync(equipmentDecommissioning.TechnicianId);

        var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
                                        .GetByIdAsync(equipmentDecommissioning.ReceptorId);

        equipmentDecommissioning.Technician = technician;
        equipmentDecommissioning.Receptor = receptor;
        equipmentDecommissioning.Equipment = equipment;

        equipmentDecommissioning.Status = DecommissioningStatus.Pending.ToString();

        await _unitOfWork.GetRepository<EquipmentDecommissioning>().CreateAsync(equipmentDecommissioning);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<EquipmentDecommissioning>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<EquipmentDecommissioningDto> UpdateAsync(EquipmentDecommissioningDto dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto.Id);

        if(equipmentDecommissioning.Status == DecommissioningStatus.Accepted.ToString())
            throw new Exception("The equipmentDecommissioning is already acepted ");

        if(dto.EquipmentId!= equipmentDecommissioning.EquipmentId)
        {
            var equipment = await _unitOfWork.GetRepository<Equipment>()
                                         .GetByIdAsync(dto.EquipmentId);
            equipmentDecommissioning.Equipment = equipment;
        }

        if(dto.ReceptorId != equipmentDecommissioning.ReceptorId)
        {
            var receptor = await _unitOfWork.GetRepository<EquipmentReceptor>()
                                        .GetByIdAsync(dto.ReceptorId);

            equipmentDecommissioning.Receptor = receptor;
        }

        if(dto.TechnicianId != equipmentDecommissioning.TechnicianId)
        {
            var technician = await _unitOfWork.GetRepository<Technician>()
                                          .GetByIdAsync(dto.TechnicianId);

            equipmentDecommissioning.Technician = technician;
        }

        _mapper.Map(dto, equipmentDecommissioning);

        _unitOfWork.GetRepository<EquipmentDecommissioning>().Update(equipmentDecommissioning);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDecommissioningDto>(equipmentDecommissioning);
    }

    public async Task AcceptDecommissioningAsync(int dto)
    {
        var equipmentDecommissioning = await _unitOfWork.GetRepository<EquipmentDecommissioning>().GetByIdAsync(dto);

        if(equipmentDecommissioning.Status == DecommissioningStatus.Accepted.ToString())
            throw new Exception("The equipmentDecomissioning is already acepted");

        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(equipmentDecommissioning.EquipmentId);

        equipment.Status = equipment.Status != EquipmentStatus.Inactive.ToString()
                            ? EquipmentStatus.Inactive.ToString()
                            : throw new Exception("The equipment is already inactive.");

        equipmentDecommissioning.Status = DecommissioningStatus.Accepted.ToString();

        _unitOfWork.GetRepository<EquipmentDecommissioning>().Update(equipmentDecommissioning);

        await _unitOfWork.CompleteAsync();

    }


}