

namespace DownTrack.Infrastructure.Authentication;

/// <summary>
/// Container for storing JWT (JSON Web Token) related configurations, which are used in user authentication and authorization.
/// </summary> 
public class JwtSettings
{
    public const string SECTION_NAME = "JwtSettings";
    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public string Audience { get; init; } = null!;
}