

using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IUnitOfWorkPattern;

/// <summary>
/// Defines the Unit of Work pattern to coordinate repository operations and ensure atomicity of changes.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for user-related operations.
    /// </summary>
    IUserRepository UserRepository { get; }

    IDepartmentRepository DepartmentRepository { get; }

    ITransferRequestRepository TransferRequestRepository { get; }

    ITransferRepository TransferRepository { get; }

    ITechnicianRepository TechnicianRepository {get;}
    
    /// <summary>
    /// Gets a generic repository for managing entities of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the entity, which must inherit from <see cref="GenericEntity"/>.</typeparam>
    /// <returns>An instance of <see cref="IGenericRepository{T}"/>.</returns>
    IGenericRepository<T> GetRepository<T>() where T : GenericEntity;

    /// <summary>
    /// Saves all changes made in the current transaction to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation, if needed.</param>
    /// <returns>A Task representing the asynchronous operation, returning the number of state entries written to the database.</returns>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
