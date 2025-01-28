
using System.Linq.Expressions;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IServices;

/// <summary>
/// Defines generic service operations for working with DTOs.
/// Provides methods for creating, updating, listing, retrieving, and deleting entities.
/// </summary>
/// <typeparam name="TDto">The type of the Data Transfer Object (DTO) used by the service.</typeparam>
public interface IGenericQueryService<TEntity,TDto> where TEntity : GenericEntity
{
    Expression<Func<TEntity,object>>[] GetIncludes();

    /// <summary>
    /// Retrieves a list of all entities.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation, returning a collection of DTOs.</returns>
    Task<IEnumerable<TDto>> ListAsync();

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
    Task<PagedResultDto<TDto>> GetPagedResultByQueryAsync(PagedRequestDto paged, IQueryable<TEntity> query);

    Task<PagedResultDto<TDto>> GetAllPagedResultAsync (PagedRequestDto paged);
}