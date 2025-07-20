using MusicTagger.Models.Database;

namespace MusicTagger.Database.Tag;

public interface ITagProvider
{
    Task<ISet<TagRecord>> GetUserTagsAsync(int userId);
}
