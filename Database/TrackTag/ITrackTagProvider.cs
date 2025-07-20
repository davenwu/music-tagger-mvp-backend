using MusicTagger.Models.Database;

namespace MusicTagger.Database.TrackTag;

public interface ITrackTagProvider
{
    Task<IList<TrackTagRecord>> GetTagsForTracksAsync(int userId, List<string> spotifyTrackIds);

    Task<IList<TrackTagRecord>> GetTracksByTagsAsync(int userId, List<long> tagIds);
}
