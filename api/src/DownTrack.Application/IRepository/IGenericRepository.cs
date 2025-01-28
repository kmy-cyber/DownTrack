
using System.Linq.Expressions;
using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;

/// <summary>
/// Defines generic repository operations for interacting with the database.
/// Provides methods for creating, updating, retrieving, and deleting entities.
/// </summary>
/// <typeparam name="T">The type of the entity managed by the repository, which must inherit from <see cref="GenericEntity"/>.</typeparam>
public interface IGenericRepository<T> where T : GenericEntity
{
    /// <summary>
    /// Asynchronously creates a new entity in the repository.
    /// </summary>
    /// <param name="element">The entity to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation, if needed.</param>
    /// <returns>A Task representing the asynchronous operation, returning the created entity.</returns>
    Task<T> CreateAsync(T element, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="element">The entity with updated values.</param>
    void Update(T element);

    /// <summary>
    /// Asynchronously retrieves an entity by its identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <param name="elementId">The identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation, if needed.</param>
    /// <param name="includes">An optional array of expressions specifying the related entities to include in the query. </param>/// 
    /// <returns>A Task representing the asynchronous operation, returning the entity with the specified identifier.</returns>
    Task<T> GetByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default, 
                            params Expression<Func<T,object>>[] includes);

                                                
    /// <summary>
    /// Retrieves all entities as an <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <returns>An <see cref="IQueryable{T}"/> representing all entities in the repository.</returns>
    IQueryable<T> GetAll();

    /// <summary>
    /// Asynchronously deletes an entity by its identifier.
    /// </summary>
    /// <param name="elementId">The identifier of the entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation, if needed.</param>
    /// <returns>A Task representing the asynchronous delete operation.</returns>
    Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchronously retrieves an entity by its identifier.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <param name="elementId">The identifier of the entity to retrieve.</param>
    /// <returns>The entity with the specified identifier.</returns>
    T GetById<TId>(TId elementId);

    /// <summary>
    /// Retrieves all entities that satisfy the specified filter expressions.
    /// </summary>
    /// <param name="expressions">A collection of filter expressions to apply to the query.</param>
    /// <returns>An <see cref="IQueryable{T}"/> containing all entities that match the provided filters.</returns>
    IQueryable<T> GetAllByItems(params Expression<Func<T,bool>>[] expressions);

    Task<T?> GetByItems (params Expression<Func<T,bool>>[] expressions);

}

