

using DownTrack.Application.IRepository;
using DownTrack.Application.IUnitOfWork;
using DownTrack.Domain.Entities;
using DownTrack.Infrastructure.Repository;

namespace DownTrack.Infrastructure.UnitOfWork;

// Pattern Unit of Work
public class UnitOfWork : IUnitOfWork
{
    private readonly DownTrackContext _context;

    private Dictionary<Type, object> _repositories;

    public UnitOfWork(DownTrackContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _repositories = new Dictionary<Type, object>();

    }

    public IGenericRepository<T> GetRepository<T>() where T : GenericEntity
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(IGenericRepository<>).MakeGenericType(type);

            try
            {
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);


                if (repositoryInstance is IGenericRepository<T> repository)
                {
                    
                    _repositories[type] = repository;
                }
                else
                {
                    throw new InvalidCastException($"The created type '{repositoryType}' is not a valid IGenericRepository<{type}>.");
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error creating repository instance", ex);
            }

        }

        return (IGenericRepository<T>)_repositories[type];
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}