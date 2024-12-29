using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;

namespace DownTrack.Infrastructure.Repository;

public class SectionRepository : GenericRepository<Section>, ISectionRepository
{
    public SectionRepository(DownTrackContext context) : base(context){}
}