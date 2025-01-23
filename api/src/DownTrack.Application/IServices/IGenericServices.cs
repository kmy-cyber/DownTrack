

using DownTrack.Application.DTO.Paged;

namespace DownTrack.Application.IServices;

/// <summary>
/// Defines generic service operations for working with DTOs.
/// Provides methods for creating, updating, listing, retrieving, and deleting entities.
/// </summary>
/// <typeparam name="TDto">The type of the Data Transfer Object (DTO) used by the service.</typeparam>
public interface IGenericService<TDto>
{
    /// <summary>
    /// Creates a new entity based on the provided DTO.
    /// </summary>
    /// <param name="dto">The DTO containing the details of the entity to create.</param>
    /// <returns>A Task representing the asynchronous operation, returning the created DTO.</returns>
    Task<TDto> CreateAsync(TDto dto);

    /// <summary>
    /// Updates an existing entity based on the provided DTO.
    /// </summary>
    /// <param name="dto">The DTO containing the updated details of the entity.</param>
    /// <returns>A Task representing the asynchronous operation, returning the updated DTO.</returns>
    Task<TDto> UpdateAsync(TDto dto);

    /// <summary>
    /// Retrieves a list of all entities.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, returning a collection of DTOs.</returns>
    Task<IEnumerable<TDto>> ListAsync();

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="dto">The identifier of the entity to delete.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(int dto);

    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    /// <param name="dto">The identifier of the entity to retrieve.</param>
    /// <returns>A Task representing the asynchronous operation, returning the DTO of the retrieved entity.</returns>
    Task<TDto> GetByIdAsync(int dto);

    /// <summary>
    /// Asynchronously retrieves a paginated list of entities .
    /// </summary>
    /// <param name="paged">The pagination parameters, including page size and page number and filters parameters</param>
    /// <returns>A Task representing the asynchronous operation, returning a paginated result as <see cref="PagedResultDto{TDto}"/>.</returns>
    Task<PagedResultDto<TDto>> GetPagedResultAsync(PagedRequestDto paged);
}
