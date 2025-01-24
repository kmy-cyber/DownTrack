

using DownTrack.Domain.Entities;

namespace DownTrack.Application.IRepository;
public interface ISectionRepository : IGenericRepository<Section>
{
    Task<Section> GetByNameAsync (string name);

}
