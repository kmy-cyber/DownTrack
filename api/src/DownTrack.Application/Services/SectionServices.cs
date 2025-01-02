using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IRepository;
using AutoMapper;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;


public class SectionServices : ISectionServices
{
    private readonly ISectionRepository _sectionRepository;
    private readonly IMapper _mapper;

    public SectionServices(ISectionRepository sectionRepository, IMapper mapper)
    {
        _sectionRepository = sectionRepository;
        _mapper = mapper;
    }

    public async Task<SectionDto> CreateAsync(SectionDto dto)
    {
        var section = _mapper.Map<Section>(dto);
        await _sectionRepository.CreateAsync(section);
        return _mapper.Map<SectionDto>(section);
    }

    public async Task DeleteAsync(int dto)
    {
        await _sectionRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<SectionDto>> ListAsync()
    {
        var section = await _sectionRepository.ListAsync();
        return section.Select(_mapper.Map<SectionDto>);
    }

    public async Task<SectionDto> UpdateAsync(SectionDto dto)
    {
        var section = _sectionRepository.GetById(dto.Id);
        _mapper.Map(dto, section);
        await _sectionRepository.UpdateAsync(section);
        return _mapper.Map<SectionDto>(section);
    }


    /// <summary>
    /// Retrieves a section by their ID
    /// </summary>
    /// <param name="sectionDto">The section's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the section</returns>
    public async Task<SectionDto> GetByIdAsync(int sectionDto)
    {
        var result = await _sectionRepository.GetByIdAsync(sectionDto);

        /// and returns the updated section as a SectionDto.
        return _mapper.Map<SectionDto>(result);

    }
}


