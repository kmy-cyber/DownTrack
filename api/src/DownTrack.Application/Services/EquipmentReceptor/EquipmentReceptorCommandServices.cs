
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EquipmentReceptorCommandServices : IEquipmentReceptorCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentReceptorCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentReceptorDto> CreateAsync(EquipmentReceptorDto dto)
    {
        var equipmentReceptor = _mapper.Map<EquipmentReceptor>(dto);

        var department = await _unitOfWork.GetRepository<Department>()
                                          .GetByIdAsync(equipmentReceptor.DepartmentId);
        
        if(dto.SectionId != department.SectionId)
            throw new Exception($"Department with Id :{department.Id} not belong to Section with Id :{dto.SectionId}");
        
        equipmentReceptor.Department = department;

        await _unitOfWork.GetRepository<EquipmentReceptor>().CreateAsync(equipmentReceptor);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentReceptorDto>(equipmentReceptor);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<EquipmentReceptor>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
       
    }

    public async Task<EquipmentReceptorDto> UpdateAsync(EquipmentReceptorDto dto)
    {
        var equipmentReceptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(dto.Id);

        if(dto.DepartmentId != equipmentReceptor.DepartmentId)
        {
            var department = await _unitOfWork.GetRepository<Department>()
                                          .GetByIdAsync(dto.DepartmentId);
        
            if(dto.SectionId != department.SectionId)
                throw new Exception($"Department with Id :{dto.Id} not belong to Section with Id :{dto.SectionId}");

            equipmentReceptor.Department = department;
        }
        
        _mapper.Map(dto, equipmentReceptor);

        _unitOfWork.GetRepository<EquipmentReceptor>().Update(equipmentReceptor);

        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<EquipmentReceptorDto>(equipmentReceptor);
    }

    
    
}