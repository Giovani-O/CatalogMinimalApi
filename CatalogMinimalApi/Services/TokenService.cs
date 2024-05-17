using CatalogMinimalApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatalogMinimalApi.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(string key, string issuer, string audience, UserModel user)
    {
        // Define claims do payload
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        // Define chave
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Geração do token
        var token = new JwtSecurityToken(
            issuer: issuer, 
            audience: audience, 
            claims: claims, 
            expires: DateTime.Now.AddMinutes(120), 
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}
