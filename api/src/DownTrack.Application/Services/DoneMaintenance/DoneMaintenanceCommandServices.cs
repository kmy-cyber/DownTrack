using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Status;

namespace DownTrack.Application.Services;
/// <summary>
/// Provides services for managing done maintenance operations.
/// </summary>
public class DoneMaintenanceCommandServices : IDoneMaintenanceCommandServices
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoneMaintenanceCommandServices"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper mapper.</param>
    public DoneMaintenanceCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new done maintenance record asynchronously.
    /// </summary>
    /// <param name="dto">The done maintenance DTO containing details to be created.</param>
    /// <returns>The created done maintenance DTO.</returns>
    public async Task<DoneMaintenanceDto> CreateAsync(DoneMaintenanceDto dto)
    {
        // Map the DTO to the domain model
        var doneMaintenance = _mapper.Map<DoneMaintenance>(dto);
        // Fetch associated entities
        doneMaintenance.Technician = await _unitOfWork.GetRepository<Technician>()
                                        .GetByIdAsync(doneMaintenance.TechnicianId!.Value);
        doneMaintenance.Equipment = await _unitOfWork.GetRepository<Equipment>()
                                        .GetByIdAsync(doneMaintenance.EquipmentId!.Value);

        // Set equipment status to UnderMaintenance
        doneMaintenance.Equipment.Status = EquipmentStatus.UnderMaintenance.ToString();
        doneMaintenance.Finish = false;

        // Create the done maintenance record
        await _unitOfWork.GetRepository<DoneMaintenance>().CreateAsync(doneMaintenance);
        await _unitOfWork.CompleteAsync();

        // Map the created domain model back to DTO
        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }
    /// <summary>
    /// Deletes a done maintenance record asynchronously.
    /// </summary>
    /// <param name="id">The ID of the done maintenance record to delete.</param>
    public async Task DeleteAsync(int dto)
    {
        // Delete the done maintenance record
        await _unitOfWork.GetRepository<DoneMaintenance>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();
    }

    /// <summary>
    /// Updates an existing done maintenance record asynchronously.
    /// </summary>
    /// <param name="dto">The done maintenance DTO containing updated details.</param>
    /// <returns>The updated done maintenance DTO.</returns>
    public async Task<DoneMaintenanceDto> UpdateAsync(DoneMaintenanceDto dto)
    {
        // Fetch the existing done maintenance record
        var doneMaintenance = await _unitOfWork.GetRepository<DoneMaintenance>()
                                               .GetByIdAsync(dto.Id);

        // Update technician if changed
        if (dto.TechnicianId != doneMaintenance.TechnicianId)
        {
            doneMaintenance.Technician = await _unitOfWork.GetRepository<Technician>()
                                                          .GetByIdAsync(dto.TechnicianId!);
        }

        // Update equipment if changed
        if (dto.EquipmentId != doneMaintenance.EquipmentId)
        {
            doneMaintenance.Equipment = await _unitOfWork.GetRepository<Equipment>()
                                                         .GetByIdAsync(dto.EquipmentId!);

            doneMaintenance.Equipment.Status = EquipmentStatus.UnderMaintenance.ToString();

        }

        // Apply updates from DTO to domain model
        _mapper.Map(dto, doneMaintenance);

        // Update the done maintenance record
        _unitOfWork.GetRepository<DoneMaintenance>().Update(doneMaintenance);

        await _unitOfWork.CompleteAsync();

        // Map the updated domain model back to DTO
        return _mapper.Map<DoneMaintenanceDto>(doneMaintenance);
    }

    /// <summary>
    /// Finalizes a maintenance operation asynchronously.
    /// </summary>
    /// <param name="requestFinalize">The finalize maintenance DTO containing details.</param>
    public async Task FinalizeMaintenanceAsync(FinalizeMaintenanceDto requestFinalize)
    {
        // Fetch the done maintenance record
        var maintenance = await _unitOfWork.GetRepository<DoneMaintenance>()
                                            .GetByIdAsync(requestFinalize.MaintenanceId, default,
                                                        m => m.Equipment!);

        // Update cost
        maintenance.Cost = requestFinalize.Cost;

        // Update equipment status
        maintenance.Equipment!.Status = EquipmentStatus.Active.ToString();

        // Mark as finished
        maintenance.Finish = true;

        // Update the done maintenance record
        _unitOfWork.GetRepository<DoneMaintenance>().Update(maintenance);

        await _unitOfWork.CompleteAsync();
    }

}