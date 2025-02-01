using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.Interfaces;
using DownTrack.Application.IServices;
using DownTrack.Application.IUnitOfWorkPattern;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services;


public class GenericQueryServices<TEntity, TDto> : IGenericQueryService<TEntity, TDto> where TEntity : GenericEntity
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    private readonly IFilterService<TEntity> _filterService;
    private readonly ISortService<TEntity> _sortService;
    private readonly IPaginationService<TEntity> _paginationService;

    public GenericQueryServices(IUnitOfWork unitOfWork,
                                IFilterService<TEntity> filterService,
                                ISortService<TEntity> sortService, 
                                IPaginationService<TEntity> paginationService,
                                IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _filterService = filterService;
        _sortService = sortService;
        _paginationService = paginationService;
        _mapper = mapper;
    }

    public virtual Expression<Func<TEntity, object>>[] GetIncludes()
    {
        return Array.Empty<Expression<Func<TEntity, object>>>();
    }

    public async Task<IEnumerable<TDto>> ListAsync()
    {
        var includes = GetIncludes();
        var dtoQuery = _unitOfWork.GetRepository<TEntity>().GetAll();

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
                                      .GetByIdAsync(dto, default, includes);

        return _mapper.Map<TDto>(result);

    }


    public async Task<PagedResultDto<TDto>> GetPagedResultByQueryAsync(PagedRequestDto paged, IQueryable<TEntity> query)
    {

        query = _filterService.ApplyFilters(query, paged.Filters!);
        
        var includes = GetIncludes();

        if (includes != null)
        {
            foreach (var exp in includes) // Loop through each filter expression.
            {
                query = query.Include(exp); // Apply the filter expression to the query.
            }
        }

        query = _sortService.ApplySort(query, paged.SortColumn!, paged.SortDescending);

        return await _paginationService.ApplyPaginationAsync<TDto>(query, paged);

    }

    public async Task<PagedResultDto<TDto>> GetAllPagedResultAsync(PagedRequestDto paged)
    {
        IQueryable<TEntity> query = _unitOfWork.GetRepository<TEntity>()
                                                .GetAll();

        return await GetPagedResultByQueryAsync(paged, query);
    }

}