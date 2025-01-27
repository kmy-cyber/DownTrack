
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EvaluationQueryServices : IEvaluationQueryServices
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EvaluationQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetEvaluationDto>> ListAsync()
    {
        var evaluation = await _unitOfWork.GetRepository<Evaluation>()
                                          .GetAll()
                                          .Include(ev => ev.SectionManager)
                                          .Include(ev => ev.Technician)
                                          .ToListAsync();


        return evaluation.Select(_mapper.Map<GetEvaluationDto>);
    }

    /// <summary>
    /// Retrieves an evaluation by their ID
    /// </summary>
    /// <param name="evaluationDto">The evaluation's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the evaluation</returns>
    public async Task<GetEvaluationDto> GetByIdAsync(int evaluationDto)
    {
        var result = await _unitOfWork.GetRepository<Evaluation>()
                                      .GetByIdAsync(evaluationDto, default,
                                      ev => ev.SectionManager!,
                                      ev => ev.Technician);

        /// and returns the updated evaluation as an EvaluationDto.
        return _mapper.Map<GetEvaluationDto>(result);

    }

    public async Task<PagedResultDto<GetEvaluationDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Evaluation> queryEvaluation = _unitOfWork.GetRepository<Evaluation>()
                                                            .GetAll()
                                                            .Include(ev => ev.SectionManager)
                                                            .Include(ev => ev.Technician);

        var totalCount = await queryEvaluation.CountAsync();

        var items = await queryEvaluation // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<GetEvaluationDto>
        {
            Items = items?.Select(_mapper.Map<GetEvaluationDto>) ?? Enumerable.Empty<GetEvaluationDto>(),
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