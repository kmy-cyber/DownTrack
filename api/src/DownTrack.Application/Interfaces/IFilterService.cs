
namespace DownTrack.Application.Interfaces;

public interface IFilterService<TEntity>
{
    IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query,Dictionary<string,object> filters);
}