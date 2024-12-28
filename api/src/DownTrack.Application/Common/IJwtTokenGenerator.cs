

using DownTrack.Domain.Enitites;

namespace DownTrack.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User userf);
}