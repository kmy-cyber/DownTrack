using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Roles;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class SectionServices : ISectionServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SectionServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<SectionDto> CreateAsync(SectionDto dto)
    {
        var section = _mapper.Map<Section>(dto);

        Employee sectionManager = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(section.SectionManagerId);

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
        //await _sectionRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<SectionDto>> ListAsync()
    {
        var section = await _unitOfWork.GetRepository<Section>().GetAll().ToListAsync();
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

    public async Task<PagedResultDto<SectionDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Section> querySection = _unitOfWork.GetRepository<Section>().GetAll();

        var totalCount = await querySection.CountAsync();

        var items = await querySection // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<SectionDto>
        {
            Items = items?.Select(_mapper.Map<SectionDto>) ?? Enumerable.Empty<SectionDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }


    public async Task<IEnumerable<DepartmentDto>> GetAllDepartments(int sectionId)
    {
        var departmentRepository = _unitOfWork.DepartmentRepository;

        //check the section exist


        var existSection = await _unitOfWork.GetRepository<Section>().GetByIdAsync(sectionId);
        // aqui se verifica que si salta una excepcion etnonce no existe sino existe la section esa

        var listDepartments = await departmentRepository.GetDepartmentsBySectionIdAsync(sectionId);

        return listDepartments.Select(_mapper.Map<DepartmentDto>);





    }
}