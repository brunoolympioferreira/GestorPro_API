using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;
using GestorPro.Domain.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GestorPro.Application.Services;

public class AuthService : IAuthService
{
    public string ComputeSha256Hash(string password)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();

        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
    }

    public TokenDTO GenerateJwtToken(User user)
    {
        var issuer = Environment.GetEnvironmentVariable("GESTOR_PRO_ISSUER");
        var audience = Environment.GetEnvironmentVariable("GESTOR_PRO_AUDIENCE");
        var key = Environment.GetEnvironmentVariable("GESTOR_PRO_KEY");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Role, user.Role.Name)
        ];

        var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        expires: DateTimeHelper.NowInBrasilia().AddHours(12),
        signingCredentials: credentials,
        claims: claims);

        var tokenHandler = new JwtSecurityTokenHandler();

        var stringToken = tokenHandler.WriteToken(token);

        return new TokenDTO(stringToken, token.ValidTo.ToLocalTime());
    }

    public bool VerifyPassword(string password, string passwordHash)
        => ComputeSha256Hash(password) == passwordHash;
}
