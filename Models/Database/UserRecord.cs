namespace MusicTagger.Models.Database;

public record UserRecord
{
    public int UserId;
    public required string Username;
    public required string PasswordHash;
}
