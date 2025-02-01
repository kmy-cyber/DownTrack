
namespace DownTrack.Application.IServices;


public interface IGenericCommandService<TDto>
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
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="dto">The identifier of the entity to delete.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    Task DeleteAsync(int dto);

}