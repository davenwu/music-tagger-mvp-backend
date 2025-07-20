using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicTagger.Auth;
using MusicTagger.Database.TrackTag;

namespace MusicTagger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackController : ControllerBase
{
    private readonly ITrackTagProvider _trackTagProvider;

    public TrackController(
        ITrackTagProvider trackTagProvider)
    {
        _trackTagProvider = trackTagProvider;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTracksByTagsAsync([FromQuery] List<long> tagIds)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue) return BadRequest("Invalid user");

        var trackTags = await _trackTagProvider.GetTracksByTagsAsync(userId.Value, tagIds);

        return Ok(trackTags.Select(tt => tt.SpotifyTrackId).Distinct());
    }
}
