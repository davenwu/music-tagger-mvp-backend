using Dapper;
using Npgsql;

namespace MusicTagger.Database.TrackTag;

public class DatabaseTrackTagWriter : ITrackTagWriter
{
    private readonly string _connectionString;

    public DatabaseTrackTagWriter(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> AssignTrackTagAsync(int userId, long tagId, string spotifyTrackId)
    {
        try
        {
            var sql = "insert into track_tags (tag_id, user_id, spotify_track_id) values (@tagId, @userId, @spotifyTrackId)";
            var values = new { tagId, userId, spotifyTrackId };

            await using var connection = new NpgsqlConnection(_connectionString);

            var rowsModified = await connection.ExecuteAsync(sql, values);

            return rowsModified > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RemoveTrackTagAsync(int userId, long tagId, string spotifyTrackId)
    {
        try
        {
            var sql = "delete from track_tags where tag_id = @tagId and user_id = @userId and spotify_track_id = @spotifyTrackId";
            var values = new { tagId, userId, spotifyTrackId };

            await using var connection = new NpgsqlConnection(_connectionString);

            var rowsModified = await connection.ExecuteAsync(sql, values);

            return rowsModified > 0;
        }
        catch
        {
            return false;
        }
    }
}
