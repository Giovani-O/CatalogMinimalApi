using CatalogMinimalApi.Models;

namespace CatalogMinimalApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(string key, string issuer, string audience, UserModel user);
    }
}
