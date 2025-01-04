
using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

// "Repository Pattern"
public class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
{
    protected readonly DbSet<T> entity;
    private DownTrackContext _context;
    public GenericRepository(DownTrackContext context)
    {
        if (context == null)
            throw new ArgumentException(nameof(context));

        _context = context;
        entity = context.Set<T>();
    }

    public virtual async Task<T> CreateAsync(T element, CancellationToken cancellationToken = default)
    {
        
        entity.Add(element);

        await _context.SaveChangesAsync(cancellationToken);

        return element;
    }

    public virtual async Task UpdateAsync(T element, CancellationToken cancellationToken = default)
    {

        entity.Update(element);

        await _context.SaveChangesAsync(cancellationToken);

    }
    public virtual async Task<T> GetByIdAsync<TId>(TId elementId, CancellationToken cancellationToken = default)
    {
        var result = await entity.FindAsync(elementId, cancellationToken);
        if (result == null)
            throw new KeyNotFoundException($"No entity was found with the ID '{elementId}'.");
        return result;
    }
    public virtual async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        var results = await entity.ToListAsync(cancellationToken);
        return results;
    }
    public virtual async Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        var result = entity.Find(elementId);
        
        if (result == null)
            throw new KeyNotFoundException($"No entity was found with the ID '{elementId}'.");

        entity.Remove(result);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public virtual T GetById<TId>(TId elementId)
    {
        return entity.Find(elementId)!;
    }
}