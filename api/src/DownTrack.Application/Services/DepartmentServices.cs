using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRepository;
using AutoMapper;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;


public class DepartmentServices : IDepartmentServices
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;

    public DepartmentServices(IDepartmentRepository departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
    {

        var existingDepartment = await _departmentRepository.GetByIdAndSectionIdAsync(dto.Id, dto.SectionId);

        if (existingDepartment != null)
        {
            throw new ConflictException($"Department '{dto.Id}' in section'{dto.SectionId}' already exists.");
        }

        var department = _mapper.Map<Department>(dto);
        await _departmentRepository.CreateAsync(department);
        return _mapper.Map<DepartmentDto>(department);

    }

    public async Task DeleteAsync(int departmentId, int sectionId)
    {
        var existingDepartment = await _departmentRepository.GetByIdAndSectionIdAsync(departmentId, sectionId);

        if (existingDepartment == null)
        {
            throw new ConflictException($"Department with ID '{departmentId}' in section '{sectionId}' does not exist.");
        }

        await _departmentRepository.DeleteAsync(existingDepartment);
    }

    public async Task<IEnumerable<DepartmentDto>> ListAsync()
    {
        var department = await _departmentRepository.ListAsync();
        return department.Select(_mapper.Map<DepartmentDto>);
    }

    public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto)
    {
        var existingDepartment = await _departmentRepository.GetByIdAndSectionIdAsync(dto.Id, dto.SectionId);

        if (existingDepartment == null)
        {
            throw new ConflictException($"Department with ID '{dto.Id}' in section '{dto.SectionId}' does not exist.");
        }

        if (dto.SectionId != existingDepartment.SectionId)
        {
            throw new InvalidOperationException("Section ID cannot be modified.");
        }

        if (dto.Id != existingDepartment.Id)
        {
            throw new InvalidOperationException("Department ID cannot be modified.");
        }

        _mapper.Map(dto, existingDepartment);
        await _departmentRepository.UpdateAsync(existingDepartment);
        return _mapper.Map<DepartmentDto>(existingDepartment);

    }
}


