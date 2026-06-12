using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WalletTracker.Application.Common.Auth;
using WalletTracker.Application.Features.Auth;
using WalletTracker.Application.Interfaces;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private static readonly JwtSecurityTokenHandler _handler = new();

    public JwtTokenGenerator(IOptions<JwtSettings> options)
    {
        _jwtSettings = options.Value;
    }

    public LoginResponse GenerateToken(User user)
    {
        // Claims
        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.UserRole.ToString()),
        };

        // Generate security key
        var symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key)
        );

        // Generate Credentials
        var credentials = new SigningCredentials(
            symmetricSecurityKey,
            SecurityAlgorithms.HmacSha256
        );

        // Audience, Issuer and Expiry Minutes
        var audience = _jwtSettings.Audience;
        var issuer = _jwtSettings.Issuer;
        var expiresAtUTC = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        // Generate Token
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAtUTC,
            signingCredentials: credentials
        );

        // Convert Token to string
        var serializedToken = _handler.WriteToken(token);

        return new LoginResponse(serializedToken, expiresAtUTC);
    }
}
