
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class EvaluationCommandServices : IEvaluationCommandServices
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    public EvaluationCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
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

    public async Task<EvaluationDto> UpdateAsync(EvaluationDto dto)
    {
        var evaluation = await _unitOfWork.GetRepository<Evaluation>().GetByIdAsync(dto.Id);

        if(dto.TechnicianId != evaluation.TechnicianId)
        {
            var technician = await _unitOfWork.GetRepository<Technician>()
                                          .GetByIdAsync(evaluation.TechnicianId);

            evaluation.Technician = technician;
        }
        
        if(dto.SectionManagerId != evaluation.SectionManagerId)
        {
            var manager = await _unitOfWork.GetRepository<Employee>()
                                           .GetByIdAsync(evaluation.SectionManagerId);

            if(manager.UserRole != "SectionManager")
            {
                throw new Exception("No es un jefe de seccion");
            }
            evaluation.SectionManager = manager;
        }

        _mapper.Map(dto, evaluation);

        _unitOfWork.GetRepository<Evaluation>().Update(evaluation);

        await _unitOfWork.CompleteAsync();
        
        return _mapper.Map<EvaluationDto>(evaluation);
    }

}