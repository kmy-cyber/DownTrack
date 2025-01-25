
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EvaluationServices : IEvaluationServices
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EvaluationServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EvaluationDto> CreateAsync(EvaluationDto dto)
    {
        var evaluation = _mapper.Map<Evaluation>(dto);

        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(evaluation.TechnicianId);

        var manager = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(evaluation.SectionManagerId);

        evaluation.Technician = technician;
        evaluation.SectionManager = manager;

        await _unitOfWork.GetRepository<Evaluation>().CreateAsync(evaluation);
        
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<EvaluationDto>(evaluation);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Evaluation>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
        
    }

    public async Task<IEnumerable<EvaluationDto>> ListAsync()
    {
        var evaluation = await _unitOfWork.GetRepository<Evaluation>().GetAll().ToListAsync();
        

        return evaluation.Select(_mapper.Map<EvaluationDto>);
    }

    public async Task<EvaluationDto> UpdateAsync(EvaluationDto dto)
    {
        var evaluation = await _unitOfWork.GetRepository<Evaluation>().GetByIdAsync(dto.Id);

        _mapper.Map(dto, evaluation);

        _unitOfWork.GetRepository<Evaluation>().Update(evaluation);

        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<EvaluationDto>(evaluation);
    }

    /// <summary>
    /// Retrieves an evaluation by their ID
    /// </summary>
    /// <param name="evaluationDto">The evaluation's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the evaluation</returns>
    public async Task<EvaluationDto> GetByIdAsync(int evaluationDto)
    {
        var result = await _unitOfWork.GetRepository<Evaluation>().GetByIdAsync(evaluationDto);

        /// and returns the updated evaluation as an EvaluationDto.
        return _mapper.Map<EvaluationDto>(result);

    }

    public async Task<PagedResultDto<EvaluationDto>> GetPagedResultAsync(PagedRequestDto paged)
    {
        //The queryable collection of entities to paginate
        IQueryable<Evaluation> queryEvaluation = _unitOfWork.GetRepository<Evaluation>().GetAll();

        var totalCount = await queryEvaluation.CountAsync();

        var items = await queryEvaluation // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<EvaluationDto>
        {
            Items = items?.Select(_mapper.Map<EvaluationDto>) ?? Enumerable.Empty<EvaluationDto>(),
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