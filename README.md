# MusicTagger
This is the backend API for MusicTagger.

MusicTagger is an application that allows you to add your own tags to songs from your Spotify library. You can then export playlists to Spotify based on any selection of tags.

# Technologies used
- ASP.NET 8
- PostgreSQL

# Deploying
> [!WARNING]
> Make sure to set the values in `appsettings.Development.json` to the appropriate values (e.g. token signing secret, connection string)

Run `dotnet run` in the root of the project to run in development mode. No other environments/mdoes are currently supported.
