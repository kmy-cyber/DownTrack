

namespace DownTrack.Application.Common.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string firstName, string lastName);
}