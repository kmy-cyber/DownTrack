using DownTrack.Application.DTO;
using DownTrack.Application.DTO.Paged;
using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

public class TechnicianRepository : GenericRepository<Technician>, ITechnicianRepository
{
    public TechnicianRepository(DownTrackContext context) : base(context){}

    public async Task<GetPagedDto<Technician>> GetPagedAsync (PagedRequestDto paged, CancellationToken cancellationToken= default)
    {
        var totalCount = await _entity.CountAsync(cancellationToken);

        var items = await _entity
                        .Skip((paged.PageNumber-1)*paged.PageSize)
                        .Take(paged.PageSize)
                        .ToListAsync(cancellationToken);
                        

        return new GetPagedDto<Technician>
        {
            Items= items,
            TotalCount = totalCount
        };

    }


}