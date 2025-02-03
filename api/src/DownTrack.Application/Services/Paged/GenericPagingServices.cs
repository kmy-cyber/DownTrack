
// using System.Linq.Expressions;
// using System.Reflection;
// using AutoMapper;
// using DownTrack.Application.DTO;
// using DownTrack.Application.DTO.Paged;
// using DownTrack.Application.IRepository;
// using DownTrack.Application.IServices;
// using DownTrack.Application.IUnitOfWorkPattern;
// using DownTrack.Domain.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;

// namespace DownTrack.Application.Services;

// public class GenericPagingServices : IGenericPagingServices
// {
//     private readonly IMapper _mapper;
//     private readonly IUnitOfWork _unitOfWork;
//     private IServiceProvider _serviceProvider;

//     public GenericPagingServices(IMapper mapper, IUnitOfWork unitOfWork,
//                                   IServiceProvider serviceProvider)
//     {
//         _mapper = mapper;
//         _unitOfWork = unitOfWork;
//         _serviceProvider = serviceProvider;
//     }


//     public async Task<PagedResultDto<TDto>> GetPagedDataAsync<TEntity,TDto>(PagedFilterRequestDto request) where TEntity : GenericEntity
//     {
//         //var repositoryType = typeof(IGenericRepository<>).MakeGenericType(Type.GetType(request.EntityName)!);
//         var repositoryType = typeof(IGenericRepository<>).MakeGenericType(typeof(TEntity));

//         var repository = (IGenericRepository<TEntity>)_serviceProvider.GetRequiredService(repositoryType);

//         var filters = ConvertFiltersToExpression<TEntity>(request.Filters);

//         var query = repository.GetAllByItems(filters);

//         var totalCount = await query.CountAsync();

//         var items = await query // Apply pagination to the query.
//                         .Skip((request.PageNumber - 1) * request.PageSize) // Skip the appropriate number of items based on the current page
//                         .Take(request.PageSize) // Take only the number of items specified by the page size.
//                         .ToListAsync(); // Convert the result to a list asynchronously.

//         //var mapping = _mapper.Map<TDto>(items);

//         return new PagedResultDto<TDto>
//         {
//             Items = items?.Select(_mapper.Map<TDto>) ?? Enumerable.Empty<TDto>(),
//             TotalCount = totalCount,
//             PageNumber = request.PageNumber,
//             PageSize = request.PageSize,
//             NextPageUrl = request.PageNumber * request.PageSize < totalCount
//                         ? $"{request.BaseUrl}?pageNumber={request.PageNumber + 1}&pageSize={request.PageSize}"
//                         : null,
//             PreviousPageUrl = request.PageNumber > 1
//                         ? $"{request.BaseUrl}?pageNumber={request.PageNumber - 1}&pageSize={request.PageSize}"
//                         : null

//         };
//     }


//     public static Expression<Func<T, bool>> ConvertFiltersToExpression<T>(Dictionary<string, object>? filters)
//     {
//         if (filters == null || filters.Count == 0)
//             return entity => true; // Si no hay filtros, devolver una expresión que siempre sea verdadera

//         ParameterExpression param = Expression.Parameter(typeof(T), "entity");
//         Expression? filterExpression = null;

//         foreach (var filter in filters)
//         {
//             string propertyName = filter.Key;
//             object? filterValue = filter.Value;

//             PropertyInfo? property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
//             if (property == null) continue; // Ignorar propiedades no encontradas

//             Expression left = Expression.Property(param, property);
//             Expression right = Expression.Constant(Convert.ChangeType(filterValue, property.PropertyType));

//             Expression condition;

//             if (property.PropertyType == typeof(string))
//             {
//                 // Usar Contains para buscar texto
//                 MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
//                 condition = Expression.Call(left, containsMethod, right);
//             }
//             else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(double) || property.PropertyType == typeof(decimal))
//             {
//                 // Igualdad para números
//                 condition = Expression.Equal(left, right);
//             }
//             else if (property.PropertyType == typeof(bool))
//             {
//                 // Comparación para booleanos
//                 condition = Expression.Equal(left, right);
//             }
//             else if (property.PropertyType == typeof(DateTime))
//             {
//                 // Comparación de fechas exactas
//                 condition = Expression.Equal(left, right);
//             }
//             else
//             {
//                 continue; // Omitir otros tipos
//             }

//             filterExpression = filterExpression == null ? condition : Expression.AndAlso(filterExpression, condition);
//         }

//         return Expression.Lambda<Func<T, bool>>(filterExpression ?? Expression.Constant(true), param);
//     }
// }

