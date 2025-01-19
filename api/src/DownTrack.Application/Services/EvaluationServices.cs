
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class EvaluationServices : IEvaluationServices
{
    //private readonly IEvaluationRepository _evaluationRepository;
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EvaluationServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _evaluationRepository = evaluationRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EvaluationDto> CreateAsync(EvaluationDto dto)
    {
        var evaluation = _mapper.Map<Evaluation>(dto);

        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(evaluation.TechnicianId);
        // ya esta garantizado que no sea null
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
        var evaluation = await _unitOfWork.GetRepository<Evaluation>().GetAllAsync().ToListAsync();
        //var evaluation = await _evaluationRepository.ListAsync();
        return evaluation.Select(_mapper.Map<EvaluationDto>);
    }

    public async Task<EvaluationDto> UpdateAsync(EvaluationDto dto)
    {
        var evaluation = await _unitOfWork.GetRepository<Evaluation>().GetByIdAsync(dto.Id);

        //var evaluation = _evaluationRepository.GetById(dto.Id);
        _mapper.Map(dto, evaluation);

        _unitOfWork.GetRepository<Evaluation>().Update(evaluation);

        await _unitOfWork.CompleteAsync();
        //await _evaluationRepository.UpdateAsync(evaluation);
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

        //var result = await _evaluationRepository.GetByIdAsync(evaluationDto);

        /// and returns the updated evaluation as an EvaluationDto.
        return _mapper.Map<EvaluationDto>(result);

    }
}