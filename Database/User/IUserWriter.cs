namespace MusicTagger.Database.User;

public interface IUserWriter
{
    Task<int?> CreateUserAsync(string username, string passwordHash);
}
