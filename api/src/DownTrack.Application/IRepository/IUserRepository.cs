

using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;


public interface IUserRepository
{
    Task<User> CreateAsync(User element, CancellationToken cancellationToken = default);
    Task<User> GetByIdEmployeeAsync(int elementId, CancellationToken cancellationToken = default);
    Task DeleteByIdEmployeeAsync(int elementId, CancellationToken cancellationToken = default);
}