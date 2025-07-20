using Dapper;
using MusicTagger.Models.Database;
using Npgsql;

namespace MusicTagger.Database.User;

public class DatabaseUserProvider : IUserProvider
{
    private readonly string _connectionString;

    public DatabaseUserProvider(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<UserRecord?> GetUserAsync(string username)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var record = await connection.QuerySingleOrDefaultAsync<UserRecord>(
            "select user_id, username, password_hash from users where username = @username",
            new { username });

        return record;
    }
}
