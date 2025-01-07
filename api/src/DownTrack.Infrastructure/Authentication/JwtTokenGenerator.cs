
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DownTrack.Application.Common.Authentication;
using DownTrack.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DownTrack.Infrastructure.Authentication;

/// <summary>
/// Provides functionality to generate JWT tokens for user authentication and authorization.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;

    public JwtTokenGenerator(JwtSettings jwtSettings, UserManager<User> userManager)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
    }

    public async Task<string> GenerateToken(User user)
    {
        //A symmetric signing key is created using the secret key defined in JwtSettings
        var key = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                SecurityAlgorithms.HmacSha256);

        // Get user role
        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Obtener el primer (y único) rol

        //claims : 
        var claims = new List<Claim>
           {
              new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
              new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              // be careful with this claim (!)
              new Claim(ClaimTypes.Role, role!)
           };


        //Creates a JwtSecurityToken object using the signing credentials, issuer, audience, claims, and expiration date.
        var securityToken = new JwtSecurityToken(
         signingCredentials: key,
         issuer: _jwtSettings.Issuer,
         audience: _jwtSettings.Audience,
         expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), //Defines when the token will expire with DateTime.UtcNow.AddMinutes
         claims: claims);

        //Converts the JwtSecurityToken object to a string representing the JWT token, ready to be sent to the client.
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}