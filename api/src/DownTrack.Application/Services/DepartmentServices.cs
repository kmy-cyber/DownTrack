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

    public async Task DeleteAsync(int dto)
    {
        var department = _departmentRepository.GetById(dto);
        if (department == null)
        {
            throw new ConflictException($"Department No.'{dto}' does not exist.");
        }
        await _departmentRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<DepartmentDto>> ListAsync()
    {
        var department = await _departmentRepository.ListAsync();
        return department.Select(_mapper.Map<DepartmentDto>);
    }

    public async Task<UpdateDepartmentDto> UpdateAsync(UpdateDepartmentDto dto)
    {
        var department = _departmentRepository.GetById(dto.Id);
        if (department == null)
        {
            throw new ConflictException($"Department '{dto.Id}' does not exist.");
        }
        _mapper.Map(dto, department);
        await _departmentRepository.UpdateAsync(department);
        return _mapper.Map<UpdateDepartmentDto>(department);

    }
}


