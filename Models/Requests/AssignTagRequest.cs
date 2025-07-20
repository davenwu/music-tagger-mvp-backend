namespace MusicTagger.Models.Requests;

public record AssignTagRequest
{
    public long TagId;
    public required string SpotifyTrackId;
}
