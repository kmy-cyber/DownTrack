
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;

public interface IGenericRepository<T> where T:GenericEntity
{
    Task<T> CreateAsync(T element, CancellationToken cancellationToken = default);
    Task UpdateAsync(T element, CancellationToken cancellationToken = default);
    Task<T> GetByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default);
    T GetById<TId>(TId elementId);
    
}