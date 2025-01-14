using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EquipmentReceptorServices : IEquipmentReceptorServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentReceptorServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _equipmentReceptorRepository = equipmentReceptorRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EquipmentReceptorDto> CreateAsync(EquipmentReceptorDto dto)
    {
        var equipmentReceptor = _mapper.Map<EquipmentReceptor>(dto);

        var department = await _unitOfWork.DepartmentRepository
                        .GetByIdAndSectionIdAsync(equipmentReceptor.DepartamentId, equipmentReceptor.SectionId);

        equipmentReceptor.Departament = department;
        
        await _unitOfWork.GetRepository<EquipmentReceptor>().CreateAsync(equipmentReceptor);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EquipmentReceptorDto>(equipmentReceptor);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<EquipmentReceptor>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
       
    }

    public async Task<IEnumerable<EquipmentReceptorDto>> ListAsync()
    {
        var equipmentReceptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetAllAsync().ToListAsync();
        
        return equipmentReceptor.Select(_mapper.Map<EquipmentReceptorDto>);
    }

    public async Task<EquipmentReceptorDto> UpdateAsync(EquipmentReceptorDto dto)
    {
        var equipmentReceptor = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(dto.Id);

        
        _mapper.Map(dto, equipmentReceptor);

        _unitOfWork.GetRepository<EquipmentReceptor>().Update(equipmentReceptor);

        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<EquipmentReceptorDto>(equipmentReceptor);
    }

    /// <summary>
    /// Retrieves a equipmentReceptor by their ID
    /// </summary>
    /// <param name="equipmentReceptorDto">The equipmentReceptor's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the equipmentReceptor</returns>
    public async Task<EquipmentReceptorDto> GetByIdAsync(int equipmentReceptorDto)
    {
        var result = await _unitOfWork.GetRepository<EquipmentReceptor>().GetByIdAsync(equipmentReceptorDto);
        
        return _mapper.Map<EquipmentReceptorDto>(result);

    }
}