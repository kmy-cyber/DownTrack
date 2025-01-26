using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TechnicianQueryServices : ITechnicianQueryServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TechnicianQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetTechnicianDto>> ListAsync()
    {
        var technician = await _unitOfWork.GetRepository<Technician>()
                                          .GetAll()
                                          .Include(t => t.User)
                                          .ToListAsync();

        return technician.Select(_mapper.Map<GetTechnicianDto>);
    }

    /// <summary>
    /// Retrieves a technician by their ID
    /// </summary>
    /// <param name="technicianDto">The technician's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the technician</returns>
    public async Task<GetTechnicianDto> GetByIdAsync(int technicianDto)
    {
        var result = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(technicianDto, default, t => t.User!);

        /// and returns the updated technician as a TechnicianDto.
        return _mapper.Map<GetTechnicianDto>(result);

    }
 
    public async Task<PagedResultDto<GetTechnicianDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Technician> queryTechnician;

        //List<Expression<Func<Technician, bool>>> exps = new List<Expression<Func<Technician, bool>>>() { exp };
        queryTechnician = _unitOfWork.GetRepository<Technician>().GetAll().Include(t=>t.User);

        var totalCount = await queryTechnician.CountAsync();

        var items = await queryTechnician // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetTechnicianDto>
        {
            Items = items?.Select(_mapper.Map<GetTechnicianDto>) ?? Enumerable.Empty<GetTechnicianDto>(),
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