using Nest;

namespace Es_Log.Services.Abstractions
{
    public interface ITokenService
    {
        Models.Token GenerateToken(string email, string id, string? role = null);
    }
}
