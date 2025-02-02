
namespace DownTrack.Application.Interfaces;

public interface ISortService<TEntity>
{
    IQueryable<TEntity> ApplySort (IQueryable<TEntity> query, string sortColumn, bool sortDescending);
}