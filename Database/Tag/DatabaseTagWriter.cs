
using Dapper;
using Npgsql;

namespace MusicTagger.Database.Tag;

public class DatabaseTagWriter : ITagWriter
{
    private readonly string _connectionString;

    public DatabaseTagWriter(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> CreateTagAsync(int userId, string tagName)
    {
        var sql = "insert into tags (tag_name, user_id) values (@tagName, @userId)";
        var values = new { tagName, userId };

        await using var connection = new NpgsqlConnection(_connectionString);

        var rowsModified = await connection.ExecuteAsync(sql, values);

        return rowsModified > 0;
    }
}
