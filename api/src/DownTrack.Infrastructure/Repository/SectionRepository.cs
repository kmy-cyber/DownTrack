using DownTrack.Application.IRepository;
using DownTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DownTrack.Infrastructure.Repository;

public class SectionRepository : GenericRepository<Section>, ISectionRepository
{
    public SectionRepository(DownTrackContext context) : base(context){}

    public async Task<Section> GetByNameAsync (string name)
    {
        var section = await _entity.SingleOrDefaultAsync(d=> d.Name == name);

        if(section is null)
            throw new Exception();
        
        return section;
    }
}