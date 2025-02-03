using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using DownTrack.Domain.Status;

namespace DownTrack.Application.Services;

/// <summary>
/// Handles commands related to equipment management, including creation, deletion, and updating of equipment records.
/// </summary>
public class EquipmentCommandServices : IEquipmentCommandServices
{
    private readonly IMapper _mapper;

    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="EquipmentCommandServices"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="mapper">The AutoMapper mapper.</param>
    public EquipmentCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new equipment record asynchronously.
    /// </summary>
    /// <param name="dto">The equipment DTO containing details to be created.</param>
    /// <returns>The created equipment DTO.</returns>
    public async Task<EquipmentDto> CreateAsync(EquipmentDto dto)
    {
        // Map the DTO to the domain model
        var equipment = _mapper.Map<Equipment>(dto);

        // Fetch department by ID
        var department = await _unitOfWork.DepartmentRepository
                                  .GetByIdAsync(equipment.DepartmentId);

        // Validate department section
        if (department.SectionId != dto.SectionId)
            throw new Exception($"Department with Id: {department.SectionId} not exist in Section with Id : {dto.SectionId}");

        // Assign department to equipment
        equipment.Department = department;

        // Validate equipment status
        if (!EquipmentStatusHelper.IsValidStatus(equipment.Status))
            throw new Exception("Invalid status");

        // Create the equipment record
        await _unitOfWork.GetRepository<Equipment>().CreateAsync(equipment);

        await _unitOfWork.CompleteAsync();
        // Map the created domain model back to DTO
        return _mapper.Map<EquipmentDto>(equipment);
    }

    /// <summary>
    /// Deletes an equipment record asynchronously.
    /// </summary>
    /// <param name="id">The ID of the equipment record to delete.</param>
    public async Task DeleteAsync(int dto)
    {
        // Delete the equipment record
        await _unitOfWork.GetRepository<Equipment>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync();

    }

    /// <summary>
    /// Updates an existing equipment record asynchronously.
    /// </summary>
    /// <param name="dto">The equipment DTO containing updated details.</param>
    /// <returns>The updated equipment DTO.</returns>
    public async Task<EquipmentDto> UpdateAsync(EquipmentDto dto)
    {
        // Fetch the existing equipment record
        var equipment = await _unitOfWork.GetRepository<Equipment>().GetByIdAsync(dto.Id);

        // Update department if changed
        if (dto.DepartmentId != equipment.DepartmentId)
        {
            var department = await _unitOfWork.DepartmentRepository
                                  .GetByIdAsync(dto.DepartmentId);

            // Validate department section
            if (department.SectionId != dto.SectionId)
                throw new Exception($"Department with Id: {department.SectionId} not exist in Section with Id : {dto.SectionId}");

            equipment.Department = department;
        }

        // Validate equipment status
        if (!EquipmentStatusHelper.IsValidStatus(dto.Status))
            throw new Exception("Invalid status");

        // Apply updates from DTO to domain model
        _mapper.Map(dto, equipment);

        // Update the equipment record
        _unitOfWork.GetRepository<Equipment>().Update(equipment);

        await _unitOfWork.CompleteAsync();

        // Map the updated domain model back to DTO
        return _mapper.Map<EquipmentDto>(equipment);
    }


}