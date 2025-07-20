namespace MusicTagger.Models.Requests;

public record class LoginRequest(
    string Username,
    string Password);
