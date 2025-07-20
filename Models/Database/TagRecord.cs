namespace MusicTagger.Models.Database;

public record TagRecord
{
    public long TagId;
    public required string TagName;
    public required int UserId;
}
