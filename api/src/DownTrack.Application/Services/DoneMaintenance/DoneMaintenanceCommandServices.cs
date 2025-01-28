using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Status;

namespace DownTrack.Application.Services;

public class DoneMaintenanceCommandServices : IDoneMaintenanceCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DoneMaintenanceCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DoneMaintenanceDto> CreateAsync(DoneMaintenanceDto dto)
    {
        var doneMaintenance = _mapper.Map<DoneMaintenance>(dto);

        doneMaintenance.Technician = await _unitOfWork.GetRepository<Technician>()
                                        .GetByIdAsync(doneMaintenance.TechnicianId!.Value);

        doneMaintenance.Equipment = await _unitOfWork.GetRepository<Equipment>()
                                        .GetByIdAsync(doneMaintenance.EquipmentId!.Value);

        doneMaintenance.Equipment.Status = EquipmentStatus.UnderMaintenance.ToString();

        await _unitOfWork.GetRepository<DoneMaintenance>().CreateAsync(doneMaintenance);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    public async Task DeleteAsync(int dto)
    {
        await _unitOfWork.GetRepository<DoneMaintenance>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<DoneMaintenanceDto> UpdateAsync(DoneMaintenanceDto dto)
    {
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>()
                                               .GetByIdAsync(dto.Id);

        if (dto.TechnicianId != doneMaintenance.TechnicianId)
        {
            doneMaintenance.Technician = await _unitOfWork.GetRepository<Technician>()
                                                          .GetByIdAsync(dto.TechnicianId!);

        }

        if (dto.EquipmentId != doneMaintenance.EquipmentId)
        {
            doneMaintenance.Equipment = await _unitOfWork.GetRepository<Equipment>()
                                                         .GetByIdAsync(dto.EquipmentId!);

            doneMaintenance.Equipment.Status = EquipmentStatus.UnderMaintenance.ToString();

        }


        _mapper.Map(dto, doneMaintenance);

        _unitOfWork.GetRepository<DoneMaintenance>().Update(doneMaintenance);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    public async Task FinalizeMaintenanceAsync(FinalizeMaintenanceDto requestFinalize)
    {
        var maintenance = await _unitOfWork.GetRepository<DoneMaintenance>()
                                            .GetByIdAsync(requestFinalize.MaintenanceId);

        maintenance.Cost = requestFinalize.Cost;

        maintenance.Equipment!.Status = EquipmentStatus.Active.ToString();

        _unitOfWork.GetRepository<DoneMaintenance>().Update(maintenance);

        await _unitOfWork.CompleteAsync();
    }

}