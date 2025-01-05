using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;

public class MaintenanceServices : IMaintenanceServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public MaintenanceServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        // _maintenanceRepository = maintenanceRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<MaintenanceDto> CreateAsync(MaintenanceDto dto)
    {
        var maintenance = _mapper.Map<Maintenance>(dto);

        //await _maintenanceRepository.CreateAsync(maintenance);
        
        await _unitOfWork.GetRepository<Maintenance>().CreateAsync(maintenance);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<MaintenanceDto>(maintenance);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<Maintenance>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
        //await _maintenanceRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<MaintenanceDto>> ListAsync()
    {
        var maintenance = await _unitOfWork.GetRepository<Maintenance>().GetAllAsync().ToListAsync();
        //var maintenance = await _maintenanceRepository.ListAsync();
        return maintenance.Select(_mapper.Map<MaintenanceDto>);
    }

    public async Task<MaintenanceDto> UpdateAsync(MaintenanceDto dto)
    {
        var maintenance = await _unitOfWork.GetRepository<Maintenance>().GetByIdAsync(dto.Id);

        //var maintenance = _maintenanceRepository.GetById(dto.Id);
        _mapper.Map(dto, maintenance);

        _unitOfWork.GetRepository<Maintenance>().Update(maintenance);

        await _unitOfWork.CompleteAsync();
        //await _maintenanceRepository.UpdateAsync(maintenance);
        return _mapper.Map<MaintenanceDto>(maintenance);
    }

    /// <summary>
    /// Retrieves a maintenance by their ID
    /// </summary>
    /// <param name="maintenanceDto">The maintenance's ID to retrieve</param>
    /// <returns>A Task representing the asynchronous operation that fetches the maintenance</returns>
    public async Task<MaintenanceDto> GetByIdAsync(int maintenanceDto)
    {
        var result = await _unitOfWork.GetRepository<Maintenance>().GetByIdAsync(maintenanceDto);
        
        //var result = await _maintenanceRepository.GetByIdAsync(maintenanceDto);

        /// and returns the updated maintenance as a MaintenanceDto.
        return _mapper.Map<MaintenanceDto>(result);

    }
}