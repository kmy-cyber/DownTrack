using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.Services;

public class SectionCommandServices : ISectionCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SectionCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<SectionDto> CreateAsync(SectionDto dto)
    {
        var section = _mapper.Map<Section>(dto);

        Employee sectionManager = await _unitOfWork.GetRepository<Employee>()
                                                   .GetByIdAsync(section.SectionManagerId);

        if (sectionManager == null)
        {
            throw new Exception($"Employee with ID {section.SectionManagerId} not found.");
        }

        if (sectionManager.UserRole != UserRole.SectionManager.ToString())
        {
            throw new Exception($"Employee with ID {section.SectionManagerId} is not a SectionManager.");
        }

        await _unitOfWork.GetRepository<Section>().CreateAsync(section);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<SectionDto>(section);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Section>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();

    }


    public async Task<SectionDto> UpdateAsync(SectionDto dto)
    {
        var section = await _unitOfWork.GetRepository<Section>().GetByIdAsync(dto.Id);

        Employee sectionManager = await _unitOfWork.GetRepository<Employee>()
                                                   .GetByIdAsync(dto.SectionManagerId);

        if (sectionManager == null)
        {
            throw new Exception($"Employee with ID {section.SectionManagerId} not found.");
        }

        if (sectionManager.UserRole != UserRole.SectionManager.ToString())
        {
            throw new Exception($"Employee with ID {section.SectionManagerId} is not a SectionManager.");
        }

        _mapper.Map(dto, section);

        _unitOfWork.GetRepository<Section>().Update(section);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<SectionDto>(section);
    }


}