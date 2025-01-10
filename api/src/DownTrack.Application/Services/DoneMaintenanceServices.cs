using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class DoneMaintenanceServices : IDoneMaintenanceServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DoneMaintenanceServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DoneMaintenanceDto> CreateAsync(DoneMaintenanceDto dto)
    {
        var doneMaintenance = _mapper.Map<DoneMaintenance>(dto);

        await _unitOfWork.GetRepository<DoneMaintenance>().CreateAsync(doneMaintenance);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<DoneMaintenance>().DeleteByIdAsync(dto);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<DoneMaintenanceDto> GetByIdAsync(int dto)
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>().GetByIdAsync(dto);
        
        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    public async Task<IEnumerable<DoneMaintenanceDto>> ListAsync()
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>().GetAllAsync().ToListAsync();
        
        return doneMaintenance.Select(_mapper.Map<DoneMaintenanceDto>);
    }

    public async Task<DoneMaintenanceDto> UpdateAsync(DoneMaintenanceDto dto)
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>().GetByIdAsync(dto.Id);
        
        _mapper.Map(dto, doneMaintenance);
        
        _unitOfWork.GetRepository<DoneMaintenance>().Update(doneMaintenance);
        
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }
}