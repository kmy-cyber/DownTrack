

using DownTrack.Application.DTO.Paged;
using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;

public interface ITechnicianRepository : IGenericRepository<Technician>
{
    Task<GetPagedDto<Technician>> GetPagedAsync (PagedRequestDto paged, CancellationToken cancellationToken= default);
}