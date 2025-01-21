
using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

// "Repository Pattern"
public class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
{
    protected readonly DbSet<T> _entity;
    private DownTrackContext _context;

    public GenericRepository(DownTrackContext context)
    {
        if (context == null)
            throw new ArgumentException(nameof(context));

        _context = context;
        _entity = _context.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T element, CancellationToken cancellationToken = default)
    {

        await _entity.AddAsync(element, cancellationToken);

        return element;
    }

    public virtual IQueryable<T> GetAllAsync()
    {
        return _entity;
    }

    public virtual void Update(T element)
    {
        _entity.Update(element);
    }


    public virtual async Task<T> GetByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default)
    {
        var result = await _entity.FindAsync(elementId, cancellationToken);
        if (result == null)
            throw new KeyNotFoundException($"No entity was found with the ID '{elementId}'.");
        return result;
    }

    public virtual async Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        var result = await GetByIdAsync(elementId,cancellationToken);

        _entity.Remove(result);
        
    }
    public virtual T GetById<TId>(TId elementId)
    {
        return _entity.Find(elementId)!;
    }
}