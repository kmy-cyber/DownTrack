using System.Linq.Expressions;
using AutoMapper;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;


public class GenericQueryServices<TEntity,TDto> : IGenericQueryService<TEntity,TDto>  where TEntity:  GenericEntity
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    public GenericQueryServices(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public virtual Expression<Func<TEntity,object>> [] GetIncludes()
    {
        return Array.Empty<Expression<Func<TEntity, object>>>();
    }
    
    public async Task<IEnumerable<TDto>> ListAsync()
    {
        var includes = GetIncludes();
        var dtoQuery =  _unitOfWork.GetRepository<TEntity>().GetAll();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                dtoQuery = dtoQuery.Include(exp); // Apply the filter expression to the query.
            }
        }

        var dtoList = await dtoQuery.ToListAsync();
        
        return dtoList.Select(_mapper.Map<TDto>);
    }


    public async Task<TDto> GetByIdAsync(int dto)
    {
        var includes = GetIncludes();

        var result = await _unitOfWork.GetRepository<TEntity>()
                                      .GetByIdAsync(dto,default,includes);

        return _mapper.Map<TDto>(result);

    }

  
    public async Task<PagedResultDto<TDto>> GetPagedResultByQueryAsync(PagedRequestDto paged, IQueryable<TEntity> query)
    {
        var includes = GetIncludes();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                query = query.Include(exp); // Apply the filter expression to the query.
            }
        }

        var totalCount = await query.CountAsync();

        var items = await query // Apply pagination to the query.
                        .Skip((paged.PageNumber - 1) * paged.PageSize) // Skip the appropriate number of items based on the current page
                        .Take(paged.PageSize) // Take only the number of items specified by the page size.
                        .ToListAsync(); // Convert the result to a list asynchronously.


        return new PagedResultDto<TDto>
        {
            Items = items?.Select(_mapper.Map<TDto>) ?? Enumerable.Empty<TDto>(),
            TotalCount = totalCount,
            PageNumber = paged.PageNumber,
            PageSize = paged.PageSize,
            NextPageUrl = paged.PageNumber * paged.PageSize < totalCount
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber + 1}&pageSize={paged.PageSize}"
                        : null,
            PreviousPageUrl = paged.PageNumber > 1
                        ? $"{paged.BaseUrl}?pageNumber={paged.PageNumber - 1}&pageSize={paged.PageSize}"
                        : null

        };
    }

    public async Task<PagedResultDto<TDto>> GetAllPagedResultAsync (PagedRequestDto paged)
    {
        IQueryable<TEntity> query = _unitOfWork.GetRepository<TEntity>()
                                                .GetAll();

        return await GetPagedResultByQueryAsync(paged,query);
    }

}