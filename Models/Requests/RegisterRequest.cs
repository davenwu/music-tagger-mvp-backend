namespace MusicTagger.Models.Requests;

public record class RegisterRequest(
    string Username,
    string Password);
