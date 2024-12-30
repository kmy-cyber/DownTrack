using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IRepository;
using DownTrack.Application.IServices;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;

public class MaintenanceServices : IMaintenanceServices
{
    private readonly IMaintenanceRepository _maintenanceRepository;
    private readonly IMapper _mapper;

    public MaintenanceServices(IMaintenanceRepository maintenanceRepository, IMapper mapper)
    {
        _maintenanceRepository = maintenanceRepository;
        _mapper = mapper;
    }

    public async Task<MaintenanceDto> CreateAsync(MaintenanceDto dto)
    {
        var maintenance = _mapper.Map<Maintenance>(dto);
        await _maintenanceRepository.CreateAsync(maintenance);
        return _mapper.Map<MaintenanceDto>(maintenance);
    }

    public async Task DeleteAsync(int dto)
    {
        await _maintenanceRepository.DeleteByIdAsync(dto);
    }

    public async Task<IEnumerable<MaintenanceDto>> ListAsync()
    {
        var maintenance = await _maintenanceRepository.ListAsync();
        return maintenance.Select(_mapper.Map<MaintenanceDto>);
    }

    public async Task<MaintenanceDto> UpdateAsync(MaintenanceDto dto)
    {
        var maintenance = _maintenanceRepository.GetById(dto.Id);
        _mapper.Map(dto, maintenance);
        await _maintenanceRepository.UpdateAsync(maintenance);
        return _mapper.Map<MaintenanceDto>(maintenance);
    }
}