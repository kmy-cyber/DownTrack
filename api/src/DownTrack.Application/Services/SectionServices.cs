using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class SectionServices : ISectionServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SectionServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _sectionRepository = sectionRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<SectionDto> CreateAsync(SectionDto dto)
    {
        var section = _mapper.Map<Section>(dto);

        //await _sectionRepository.CreateAsync(section);
        
        await _unitOfWork.GetRepository<Section>().CreateAsync(section);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<SectionDto>(section);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Section>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
        //await _sectionRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<SectionDto>> ListAsync()
    {
        var section = await _unitOfWork.GetRepository<Section>().GetAllAsync().ToListAsync();
        //var section = await _sectionRepository.ListAsync();
        return section.Select(_mapper.Map<SectionDto>);
    }

    public async Task<SectionDto> UpdateAsync(SectionDto dto)
    {
        var section = await _unitOfWork.GetRepository<Section>().GetByIdAsync(dto.Id);

        //var section = _sectionRepository.GetById(dto.Id);
        _mapper.Map(dto, section);

        _unitOfWork.GetRepository<Section>().Update(section);

        await _unitOfWork.CompleteAsync();
        //await _sectionRepository.UpdateAsync(section);
        return _mapper.Map<SectionDto>(section);
    }

    /// <summary>
    /// Retrieves a section by their ID
    /// </summary>
    /// <param name="sectionDto">The section's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the section</returns>
    public async Task<SectionDto> GetByIdAsync(int sectionDto)
    {
        var result = await _unitOfWork.GetRepository<Section>().GetByIdAsync(sectionDto);
        
        //var result = await _sectionRepository.GetByIdAsync(sectionDto);

        /// and returns the updated section as a SectionDto.
        return _mapper.Map<SectionDto>(result);

    }
}