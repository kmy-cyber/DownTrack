

using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;


public interface IUserRepository
{
    Task<User> CreateAsync(User element, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(int elementId, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default);
}