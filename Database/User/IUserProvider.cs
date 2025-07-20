using MusicTagger.Models.Database;

namespace MusicTagger.Database.User;

public interface IUserProvider
{
    Task<UserRecord?> GetUserAsync(string username);
}
