namespace MusicTagger.Database.TrackTag;

public interface ITrackTagWriter
{
    Task<bool> AssignTrackTagAsync(int userId, long tagId, string spotifyTrackId);

    Task<bool> RemoveTrackTagAsync(int userId, long tagId, string spotifyTrackId);
}
