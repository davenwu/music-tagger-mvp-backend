namespace MusicTagger.Auth;

public interface ITokenGenerator
{
    string GenerateToken(string username, int userId);
}
