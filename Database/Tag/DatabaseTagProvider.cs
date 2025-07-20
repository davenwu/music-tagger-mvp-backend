using Dapper;
using MusicTagger.Models.Database;
using Npgsql;

namespace MusicTagger.Database.Tag;

public class DatabaseTagProvider : ITagProvider
{
    private readonly string _connectionString;

    public DatabaseTagProvider(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<ISet<TagRecord>> GetUserTagsAsync(int userId)
    {
        var sql = "select tag_id, tag_name, user_id from tags where user_id = @userId";
        var values = new { userId };

        await using var connection = new NpgsqlConnection(_connectionString);

        var tags = await connection.QueryAsync<TagRecord>(sql, values);

        return tags.ToHashSet();
    }
}
