

using DownTrack.Application.IRespository;
using DownTrack.Domain.Enitites;

namespace DownTrack.Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DownTrackContext context) : base(context) { }
}