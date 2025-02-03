using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

/// <summary>
/// Defines the contract for querying equipment-related data.
/// This interface provides methods for retrieving paginated results of equipment based on various criteria.
/// </summary>
public interface IEquipmentQueryServices : IGenericQueryService<Equipment, GetEquipmentDto>
{

    /// <summary>
    /// Retrieves paginated equipment records filtered by section manager ID.
    /// </summary>
    /// <param name="paged">The paged request DTO containing pagination parameters.</param>
    /// <param name="sectionManagerId">The ID of the section manager to filter equipment by.</param>
    /// <returns>A PagedResultDto containing the Equipment that belong to section/sections of a SectionManager.</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionManagerIdAsync(PagedRequestDto paged, int sectionManagerId);

    /// <summary>
    /// Retrieves paginated equipment records filtered by section ID.
    /// </summary>
    /// <param name="paged">The paged request DTO containing pagination parameters.</param>
    /// <param name="sectionId">The ID of the section to filter equipment by.</param>
    /// <returns>A PagedResultDto containing the equipment that belongs to the section with id <param name="sectionId"> .</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsBySectionIdAsync(PagedRequestDto paged, int sectionId);

    /// <summary>
        /// Retrieves paginated equipment records filtered by department ID.
        /// </summary>
        /// <param name="paged">The paged request DTO containing pagination parameters.</param>
        /// <param name="departmentId">The ID of the department to filter equipment by.</param>
        /// <returns>A PagedResultDto containing the equipment that belong to a department.</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByDepartmentIdAsync(PagedRequestDto paged, int departmentId);

    /// <summary>
        /// Retrieves paginated equipment records filtered by equipment name.
        /// </summary>
        /// <param name="paged">The paged request DTO containing pagination parameters.</param>
        /// <param name="equipmentName">The name of the equipment to filter by.</param>
        /// <returns>A PagedResultDto containing the equipment with the name given as a parameter.</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByNameAsync(PagedRequestDto aged, string equipmentName);

    /// <summary>
        /// Retrieves paginated active equipment records.
        /// </summary>
        /// <param name="paged">The paged request DTO containing pagination parameters.</param>
        /// <param name="equipmentName">The name of the equipment.</param>
        /// <param name="sectionManagerId">The Id of the manager.</param>
        /// <returns>A PagedResultDto containing the queried active equipment details.</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsByNameAndSectionManagerAsync(PagedRequestDto paged, string equipmentName, int sectionManagerId);

/// <summary>
        /// Retrieves paginated active equipment records.
        /// </summary>
        /// <param name="paged">The paged request DTO containing pagination parameters.</param>
        /// <returns>A PagedResultDto containing the queried active equipment details.</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetActiveEquipment(PagedRequestDto paged);

    /// <summary>
        /// Retrieves paginated equipment records with more than three maintenance records within the last year.
        /// </summary>
        /// <param name="paged">The paged request DTO containing pagination parameters.</param>
        /// <returns>A PagedResultDto containing the equipment that have recieved more than three maintenances in the last year.</returns>
    Task<PagedResultDto<GetEquipmentDto>> GetPagedEquipmentsWith3MaintenancesAsync(PagedRequestDto paged);
    Task<PagedResultDto<GetEquipmentDto>> GetTransferredEquipmentsByDepartmentAsync(PagedRequestDto paged,int departmentId);

}
