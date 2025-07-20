using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MusicTagger.Auth;

public static class JwtSigningKey
{
    private const string AUTH_TOKEN_SIGNING_SECRET = "Auth:TokenSigningSecret";

    private static SecurityKey? _key = null;

    public static SecurityKey GetKey(IConfiguration c)
    {
        _key ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(c[AUTH_TOKEN_SIGNING_SECRET]));

        return _key;
    }
}
