using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Status;

namespace DownTrack.Application.Services;

public class EquipmentCommandServices : IEquipmentCommandServices
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EquipmentCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentDto> CreateAsync(EquipmentDto dto)
    {
        var equipment = _mapper.Map<Equipment>(dto);

        var department = await _unitOfWork.DepartmentRepository
                                  .GetByIdAsync(equipment.DepartmentId);

        if(department.SectionId != dto.SectionId)
            throw new Exception($"Department with Id: {department.SectionId} not exist in Section with Id : {dto.SectionId}");

        equipment.Department = department;

        if(!EquipmentStatusHelper.IsValidStatus(equipment.Status))
            throw new Exception("Invalid status");
            
        await _unitOfWork.GetRepository<Equipment>().CreateAsync(equipment);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentDto>(equipment);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Equipment>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
       
    }

    public async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(dto.Id);

        if(dto.DepartmentId != equipment.DepartmentId)
        {
            var department = await _unitOfWork.DepartmentRepository
                                  .GetByIdAsync(dto.DepartmentId);

            if(department.SectionId != dto.SectionId)
                throw new Exception($"Department with Id: {department.SectionId} not exist in Section with Id : {dto.SectionId}");

            equipment.Department = department;
        }

        if(!EquipmentStatusHelper.IsValidStatus(dto.Status))
            throw new Exception("Invalid status");
        
        _mapper.Map(dto, equipment);

        _unitOfWork.GetRepository<Equipment>().Update(equipment);

        await _unitOfWork.CompleteAsync();
       
        return _mapper.Map<EquipmentDto>(equipment);
    }


}