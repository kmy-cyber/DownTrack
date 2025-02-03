
using AutoMapper;
using DownTrack.Application.DTO;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.Services;


/// <summary>
/// Service class for handling business logic related to departments.
/// Interacts with repositories and uses DTOs for client communication.
/// </summary>
public class DepartmentCommandServices : IDepartmentCommandServices
{

    // Automapper instance for mapping between domain entities and DTOs.
    private readonly IMapper _mapper;

    // Unit of Work instance for managing repositories and transactions.
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentCommandServices(IUnitOfWork unitOfWork, IMapper mapper)
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

        var departmentRepository = _unitOfWork.DepartmentRepository;
        
        bool existDepartment = await departmentRepository
                                    .ExistsByNameAndSectionAsync(department.Name,department.SectionId);

        if(existDepartment)
            throw new Exception("A department with the same name already exists in this section.");

        department.Section = await _unitOfWork.GetRepository<Section>().GetByIdAsync(dto.SectionId);

        //Adds the new department to the repository.
        await _unitOfWork.GetRepository<Department>().CreateAsync(department);

        //Commits the transaction.
        await _unitOfWork.CompleteAsync();

        // Maps the created entity back to DTO.
        return _mapper.Map<DepartmentDto>(department);

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
    /// Updates an existing department's information.
    /// </summary>
    /// <param name="dto">The DepartmentDto containing updated details.</param>
    /// <returns>The updated DepartmentDto.</returns>
    public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto)
    {

        var existingDepartment = await _unitOfWork.GetRepository<Department>().GetByIdAsync(dto.Id);

        if(dto.Id!= existingDepartment.Id || dto.SectionId != existingDepartment.SectionId)
        {
            bool existDepartment = await _unitOfWork.DepartmentRepository
                                    .ExistsByIdAndSectionAsync(dto.Id,dto.SectionId);
            if(existDepartment)
                throw new Exception($"Department '{dto.Id}' does not exist in Section '{dto.SectionId}' ");

        }
            
        _mapper.Map(dto, existingDepartment);

        _unitOfWork.GetRepository<Department>().Update(existingDepartment);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<DepartmentDto>(existingDepartment);

    }


}