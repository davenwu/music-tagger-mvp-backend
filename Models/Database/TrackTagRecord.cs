namespace MusicTagger.Models.Database;

public record TrackTagRecord
{
    public long TagId;
    public int UserId;
    public required string SpotifyTrackId;
}
