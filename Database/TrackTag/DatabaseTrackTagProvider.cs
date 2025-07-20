using Dapper;
using MusicTagger.Models.Database;
using Npgsql;

namespace MusicTagger.Database.TrackTag;

public class DatabaseTrackTagProvider : ITrackTagProvider
{
    private readonly string _connectionString;
    public DatabaseTrackTagProvider(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IList<TrackTagRecord>> GetTagsForTracksAsync(int userId, List<string> spotifyTrackIds)
    {
        try
        {
            var sql = @"
                select
                    tag_id,
                    user_id,
                    spotify_track_id
                from track_tags
                where user_id = @userId and spotify_track_id = any(@spotifyTrackIds)
                ";
            var values = new { userId, spotifyTrackIds = spotifyTrackIds.ToArray() };

            await using var connection = new NpgsqlConnection(_connectionString);

            var trackTags = await connection.QueryAsync<TrackTagRecord>(sql, values);

            return trackTags.ToList();
        }
        catch
        {
            return new List<TrackTagRecord>();
        }
    }

    public async Task<IList<TrackTagRecord>> GetTracksByTagsAsync(int userId, List<long> tagIds)
    {
        try
        {
            var sql = @"
                select
                    tag_id,
                    user_id,
                    spotify_track_id
                from track_tags
                where user_id = @userId and tag_id = any(@tagIds)
                ";
            var values = new { userId, tagIds = tagIds.ToArray() };

            await using var connection = new NpgsqlConnection(_connectionString);

            var trackTags = await connection.QueryAsync<TrackTagRecord>(sql, values);

            return trackTags.ToHashSet().ToList();
        }
        catch
        {
            return new List<TrackTagRecord>();
        }
    }
}
