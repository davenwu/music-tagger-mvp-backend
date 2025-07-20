using System.Security.Claims;

namespace MusicTagger.Auth;

public static class UserUtil
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(userIdClaim, out int userId) ? userId : null;
    }

    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name);
    }
}
