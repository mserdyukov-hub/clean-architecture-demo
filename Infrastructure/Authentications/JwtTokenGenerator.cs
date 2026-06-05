using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentications;

public class JwtTokenGenerator(IOptions<JwtSettings> jwtSettings) : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    /// <summary>
    /// Генерирует JWT токен для пользователя.
    /// В токен включаются: userId, email, username, роли и разрешения пользователя
    /// </summary>
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new("username", user.UserName),
            // Защита от повторного использования, Отзыв токенов, Аудит и логирование
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Добавление ролей
        claims.AddRange(user.UserRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole.Role.Name)));

        var permissions = user.UserRoles
            .SelectMany(ur => ur.Role.RolePermissions)
            .Select(rp => rp.Permission.Code)
            .Distinct();

        // Добавление разрешений
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
