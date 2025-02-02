using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class TechnicianCommandServices : ITechnicianCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TechnicianCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
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

    public async Task<TechnicianDto> UpdateAsync(TechnicianDto dto)
    {
        Console.WriteLine(dto.Id);
        var technician = await _unitOfWork.GetRepository<Technician>().GetByIdAsync(dto.Id);
        Console.WriteLine(dto.Id);
        _mapper.Map(dto, technician);

        _unitOfWork.GetRepository<Technician>().Update(technician);
        Console.WriteLine(dto.Id);
        await _unitOfWork.CompleteAsync();
        Console.WriteLine(dto.Id);
        return _mapper.Map<TechnicianDto>(technician);
    }


}