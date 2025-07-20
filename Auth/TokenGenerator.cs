using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace MusicTagger.Auth;

public class TokenGenerator : ITokenGenerator
{
    private readonly SigningCredentials _tokenSigningCredentials;
    private readonly JsonWebTokenHandler _tokenHandler;
    private int _tokenExpirationHours;
    private readonly string _tokenIssuer;

    public TokenGenerator(
        SigningCredentials tokenSigningCredentials,
        JsonWebTokenHandler tokenHandler,
        int tokenExpirationHours,
        string tokenIssuer)
    {
        _tokenSigningCredentials = tokenSigningCredentials;
        _tokenHandler = tokenHandler;
        _tokenExpirationHours = tokenExpirationHours;
        _tokenIssuer = tokenIssuer;
    }

    public string GenerateToken(string username, int userId)
    {
        var expirationTime = DateTime.UtcNow.AddHours(_tokenExpirationHours);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _tokenIssuer,
            Audience = _tokenIssuer,
            Claims = new Dictionary<string, object>
            {
                [ClaimTypes.Name] = username,
                [ClaimTypes.NameIdentifier] = userId
            },
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = expirationTime,
            SigningCredentials = _tokenSigningCredentials
        };

        return _tokenHandler.CreateToken(descriptor);
    }
}
