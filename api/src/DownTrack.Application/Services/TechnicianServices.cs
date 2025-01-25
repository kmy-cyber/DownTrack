using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class TechnicianServices : ITechnicianServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TechnicianServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<TechnicianDto> CreateAsync(TechnicianDto dto)
    {
        var technician = _mapper.Map<Technician>(dto);

        await _unitOfWork.GetRepository<Technician>().CreateAsync(technician);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TechnicianDto>(technician);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Technician>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();

    }

    public async Task<IEnumerable<TechnicianDto>> ListAsync()
    {
        var technician = await _unitOfWork.GetRepository<Technician>().GetAll().ToListAsync();
        //var technician = await _technicianRepository.ListAsync();
        return technician.Select(_mapper.Map<TechnicianDto>);
    }

    public async Task<TechnicianDto> UpdateAsync(TechnicianDto dto)
    {
        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(dto.Id);

        //var technician = _technicianRepository.GetById(dto.Id);
        _mapper.Map(dto, technician);

        _unitOfWork.GetRepository<Technician>().Update(technician);

        await _unitOfWork.CompleteAsync();
        //await _technicianRepository.UpdateAsync(technician);
        return _mapper.Map<TechnicianDto>(technician);
    }

    /// <summary>
    /// Retrieves a technician by their ID
    /// </summary>
    /// <param name="technicianDto">The technician's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the technician</returns>
    public async Task<TechnicianDto> GetByIdAsync(int technicianDto)
    {
        var result = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(technicianDto);

        /// and returns the updated technician as a TechnicianDto.
        return _mapper.Map<TechnicianDto>(result);

    }

    public async Task<PagedResultDto<TechnicianDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        return await GetPagedResultWithFilterAsync(paged,null);
    }
    public async Task<PagedResultDto<TechnicianDto>> GetPagedResultWithFilterAsync(PagedRequestDto paged,
                                                                        Expression<Func<Technician, bool>>? exp)
    {
        //The queryable collection of entities to paginate
        IQueryable<Technician> queryTechnician;

        if (exp is null)
        {
            queryTechnician = _unitOfWork.GetRepository<Technician>().GetAll();

        }
        else
        {
            List<Expression<Func<Technician, bool>>> exps = new List<Expression<Func<Technician, bool>>>() { exp };
            queryTechnician = _unitOfWork.GetRepository<Technician>().GetAllByItems(exps);
        }



        var totalCount = await queryTechnician.CountAsync();

        var items = await queryTechnician // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<TechnicianDto>
        {
            Items = items?.Select(_mapper.Map<TechnicianDto>) ?? Enumerable.Empty<TechnicianDto>(),
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