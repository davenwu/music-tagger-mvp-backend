namespace MusicTagger.Database.Tag;

public interface ITagWriter
{
    Task<bool> CreateTagAsync(int userId, string tagName);
}
