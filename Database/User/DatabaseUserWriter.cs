using Dapper;
using Npgsql;

namespace MusicTagger.Database.User;

public class DatabaseUserWriter : IUserWriter
{
    private readonly string _connectionString;

    public DatabaseUserWriter(
        string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int?> CreateUserAsync(string username, string passwordHash)
    {
        try
        {
            var sql = "insert into users (username, password_hash) values (@username, @passwordHash) returning user_id";
            var values = new { username, passwordHash };

            await using var connection = new NpgsqlConnection(_connectionString);
            var userId = await connection.ExecuteScalarAsync<int>(sql, values);

            return userId;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
