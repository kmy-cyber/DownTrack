using AutoMapper;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Application.Services.Specials;

public class PaginationService<TEntity> : IPaginationService<TEntity>
{
    private readonly IMapper _mapper;

    public PaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<PagedResultDto<TDto>> ApplyPaginationAsync<TDto>(IQueryable<TEntity> query, PagedRequestDto request)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PagedResultDto<TDto>
        {
            Items = items?.Select(item => _mapper.Map<TDto>(item)) ?? Enumerable.Empty<TDto>(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            NextPageUrl = request.PageNumber * request.PageSize < totalCount
                        ? $"{request.BaseUrl}?pageNumber={request.PageNumber + 1}&pageSize={request.PageSize}"
                        : null,
            PreviousPageUrl = request.PageNumber > 1
                        ? $"{request.BaseUrl}?pageNumber={request.PageNumber - 1}&pageSize={request.PageSize}"
                        : null
        };
    }
}