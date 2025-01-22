using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;


/// <summary>
/// Service class for handling business logic related to departments.
/// Interacts with repositories and uses DTOs for client communication.
/// </summary>
public class DepartmentServices : IDepartmentServices
{

    // Automapper instance for mapping between domain entities and DTOs.
    private readonly IMapper _mapper;

    // Unit of Work instance for managing repositories and transactions.
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }


    /// <summary>
    /// Creates a new department based on the provided DTO.
    /// </summary>
    /// <param name="dto">The DepartmentDto containing the department details to create.</param>
    /// <returns>The created DepartmentDto.</returns>
    public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
    {

        //Maps DTO to domain entity.

        var department = _mapper.Map<Department>(dto);
        department.SectionId = dto.SectionId;
        department.Section = await _unitOfWork.GetRepository<Section>().GetByIdAsync(dto.SectionId);
        //Adds the new department to the repository.
        await _unitOfWork.GetRepository<Department>().CreateAsync(department);

        //Commits the transaction.
        await _unitOfWork.CompleteAsync();

        // Maps the created entity back to DTO.
        return _mapper.Map<DepartmentDto>(department);

    }

    public async Task DeleteAsync(int departmentId, int sectionId)
    {

        await _unitOfWork.DepartmentRepository.DeleteAsync(departmentId, sectionId);

        await _unitOfWork.CompleteAsync();
    }



    /// <summary>
    /// Deletes a department by its ID.
    /// </summary>
    /// <param name="dto">The ID of the department to delete.</param>
    public async Task DeleteAsync(int dto)
    {
        // Removes the department by its ID
        await _unitOfWork.GetRepository<Department>().DeleteByIdAsync(dto);

        await _unitOfWork.CompleteAsync(); // Commits the transaction.
    }


    /// <summary>
    /// Retrieves a list of all departments along with their section names.
    /// </summary>
    /// <returns>A collection of DepartmentDto representing all departments with section names.</returns>
    public async Task<IEnumerable<DepartmentPresentationDto>> ListAsync()
    {
        var departments = await _unitOfWork
            .GetRepository<Department>()
            .GetAllAsync() // Devuelve IQueryable<Department>
            .Include(d => d.Section) // Incluye la relación con Section
            .ToListAsync(); // Ejecuta la consulta y materializa los resultados

        return departments.Select(department => new DepartmentPresentationDto
        {
            Id = department.Id,
            Name = department.Name,
            SectionId = department.SectionId,
            SectionName = department.Section.Name // Incluye el nombre de la sección
        });
    }



    /// <summary>
    /// Updates an existing department's information.
    /// </summary>
    /// <param name="dto">The DepartmentDto containing updated details.</param>
    /// <returns>The updated DepartmentDto.</returns>
    public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto)
    {

        var existingDepartment = await _unitOfWork.DepartmentRepository.GetByIdAndSectionIdAsync(dto.Id, dto.SectionId);

        if (existingDepartment == null)
        {
            throw new ConflictException($"Department with ID '{dto.Id}' in section '{dto.SectionId}' does not exist.");
        }


        _mapper.Map(dto, existingDepartment);

        _unitOfWork.DepartmentRepository.Update(existingDepartment);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DepartmentDto>(existingDepartment);

    }



    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="departmentDto">The ID of the department to retrieve.</param>
    /// <returns>The DepartmentDto of the retrieved department.</returns>
    public async Task<DepartmentDto> GetByIdAsync(int departmentDto)
    {
        var result = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentDto);

        return _mapper.Map<DepartmentDto>(result);

    }



}