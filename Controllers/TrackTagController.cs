using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicTagger.Auth;
using MusicTagger.Database.TrackTag;
using MusicTagger.Models.Requests;

namespace MusicTagger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackTagController : ControllerBase
{
    private readonly ITrackTagProvider _trackTagProvider;
    private readonly ITrackTagWriter _trackTagWriter;

    public TrackTagController(
        ITrackTagProvider trackTagProvider,
        ITrackTagWriter trackTagWriter)
    {
        _trackTagProvider = trackTagProvider;
        _trackTagWriter = trackTagWriter;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTagsForTracksAsync([FromQuery] List<string> spotifyTrackIds)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue) return BadRequest("Invalid user");

        var trackTags = await _trackTagProvider.GetTagsForTracksAsync(userId.Value, spotifyTrackIds);

        return Ok(trackTags);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AssignTrackTagAsync([FromBody] AssignTagRequest request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue) return BadRequest();

        var tagAssigned = await _trackTagWriter.AssignTrackTagAsync(userId.Value, request.TagId, request.SpotifyTrackId);
        if (!tagAssigned) return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> RemoveTrackTagAsync([FromBody] AssignTagRequest request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue) return BadRequest();

        var tagAssigned = await _trackTagWriter.RemoveTrackTagAsync(userId.Value, request.TagId, request.SpotifyTrackId);
        if (!tagAssigned) return StatusCode(StatusCodes.Status500InternalServerError);

        return Ok();
    }

    /*
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SetTagsForTrackAsync([FromBody] SetTagsForTrackRequest request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue) return BadRequest();

        var addTagsTask = _trackTagWriter.AddTrackTagsAsync(userId.Value, request.TagIds, request.SpotifyTrackId);
        var removeTagsTask = _trackTagWriter.RemoveTrackTagsAsync(userId.Value, request.TagIds, request.SpotifyTrackId);
        var tagsAdded = await addTagsTask;
        var tagsRemoved = await removeTagsTask;

        var errors = new List<string>();
        if (!tagsAdded) errors.Add("Failed to add all tags to track");
        if (!tagsRemoved) errors.Add("Failed to remove all tags from track");
        if (errors.Count > 0) return StatusCode(StatusCodes.Status500InternalServerError, errors);

        return Ok();
    }
    */
}
