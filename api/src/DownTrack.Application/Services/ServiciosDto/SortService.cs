using System.Linq.Expressions;
using DownTrack.Application.Interfaces;

namespace DownTrack.Application.Services.Specials;

public class SortService<TEntity> : ISortService<TEntity>
{
    public IQueryable<TEntity> ApplySort(IQueryable<TEntity> query, string sortColumn, bool sortDescending)
    {
        if (string.IsNullOrEmpty(sortColumn)) return query;

        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, sortColumn);
        var lambda = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(property, typeof(object)), parameter);

        return sortDescending ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
    }
}