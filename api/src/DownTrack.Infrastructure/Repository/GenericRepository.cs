
using System.Linq.Expressions;
using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

// "Repository Pattern"
public class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
{
    protected readonly DbSet<T> _entity; // Represents the database set for the entity type T.
    private DownTrackContext _context; // Holds the database context for interacting with the database.

    public GenericRepository(DownTrackContext context)
    {
        if (context == null)
            throw new ArgumentException(nameof(context));

        _context = context;
        _entity = _context.Set<T>();// Initialize the DbSet for the entity type T.
    }

    public virtual async Task<T> CreateAsync(T element, CancellationToken cancellationToken = default)
    {
        // Asynchronously add the entity to the DbSet.
        await _entity.AddAsync(element, cancellationToken);

        // Return the added entity
        return element;
    }

    public virtual IQueryable<T> GetAll()
    {
        return _entity; // Return the entire DbSet as an IQueryable.
    }

    public virtual void Update(T element)
    {
       Console.WriteLine(element);
        _entity.Update(element); // Update the provided entity in the DbSet.
        
    }


    public virtual async Task<T> GetByIdAsync<TId>(TId elementId,
                                                    CancellationToken cancellationToken = default,
                                                    params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _entity;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        var result = await query.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(elementId), cancellationToken);
        Console.WriteLine(elementId);
        Console.WriteLine(result);
        if (result == null) // Check if the entity was not found.
            throw new KeyNotFoundException($"No entity was found with the ID '{elementId}'.");

        return result;
    }

    public virtual async Task DeleteByIdAsync(int elementId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine(elementId);
        // Retrieve the entity by its ID.
        var result = await GetByIdAsync(elementId, cancellationToken);

        // Remove the retrieved entity from the DbSet.
        _entity.Remove(result);

    }
    public virtual T GetById<TId>(TId elementId)
    {
        return _entity.Find(elementId)!; // Synchronously find and return the entity by its ID.
    }


    public virtual IQueryable<T> GetAllByItems(params Expression<Func<T, bool>>[] expressions)
    {
        IQueryable<T> query = _entity; // Initialize the query with the DbSet.

        if (expressions != null)
        {
            foreach (var exp in expressions) // Loop through each filter expression.
            {
                query = query.Where(exp); // Apply the filter expression to the query.
            }
        }

        // Return the filtered query.
        return query;
    }


    public async Task<T?> GetByItems(Expression<Func<T, bool>>[] expressions,
                                     Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _entity;



        if (expressions != null)
        {
            foreach (var exp in expressions) // Loop through each filter expression.
            {
                query = query.Where(exp); // Apply the filter expression to the query.
            }
        }

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include); // Apply each include to the query.
            }
        }

        var result = await query.FirstOrDefaultAsync();


        return result;
    }


}