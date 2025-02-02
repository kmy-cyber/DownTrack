

using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;


public interface IUserRepository
{
    Task<User> GetByIdAsync(int elementId, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default);
    Task UpdateByIdAsync(int elementId, string email, CancellationToken cancellationToken = default);
}