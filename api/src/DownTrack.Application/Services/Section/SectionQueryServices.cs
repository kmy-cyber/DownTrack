using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class SectionQueryServices : ISectionQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SectionQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetSectionDto>> ListAsync()
    {
        var section = await _unitOfWork.GetRepository<Section>()
                                       .GetAll()
                                       .Include(s=> s.SectionManager.User)
                                       .ToListAsync();
        
        return section.Select(_mapper.Map<GetSectionDto>);
    }

    /// <summary>
    /// Retrieves a section by their ID
    /// </summary>
    /// <param name="sectionDto">The section's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the section</returns>
    public async Task<GetSectionDto> GetByIdAsync(int sectionDto)
    {
        var result = await _unitOfWork.GetRepository<Section>()
                                      .GetByIdAsync(sectionDto,default, s=> s.SectionManager.User!);
        /// and returns the updated section as a SectionDto.
        return _mapper.Map<GetSectionDto>(result);

    }

    public async Task<PagedResultDto<GetSectionDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Section> querySection = _unitOfWork.GetRepository<Section>()
                                                      .GetAll()
                                                      .Include(s=> s.SectionManager.User);

        var totalCount = await querySection.CountAsync();

        var items = await querySection // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetSectionDto>
        {
            Items = items?.Select(_mapper.Map<GetSectionDto>) ?? Enumerable.Empty<GetSectionDto>(),
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

    public async Task<PagedResultDto<GetSectionDto>> GetSectionsByManageAsync(PagedRequestDto paged, int sectionManagerId)
    {
        //check if a valid section
        var section = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(sectionManagerId);

        if(section.UserRole != "SectionManager")
            throw new Exception($"This SectionManager with Id : {sectionManagerId} not exist");
            
        //The queryable collection of entities to paginate
        IQueryable<Section> querySection = _unitOfWork.GetRepository<Section>()
                                                      .GetAllByItems(s=> s.SectionManagerId == sectionManagerId)
                                                      .Include(s=> s.SectionManager.User);

        var totalCount = await querySection.CountAsync();

        var items = await querySection // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetSectionDto>
        {
            Items = items?.Select(_mapper.Map<GetSectionDto>) ?? Enumerable.Empty<GetSectionDto>(),
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

}