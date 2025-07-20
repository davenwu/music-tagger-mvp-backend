using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicTagger.Auth;
using MusicTagger.Database.Tag;
using MusicTagger.Models.Requests;

namespace MusicTagger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagProvider _tagProvider;
    private readonly ITagWriter _tagWriter;

    public TagController(
        ITagProvider tagProvider,
        ITagWriter tagWriter)
    {
        _tagProvider = tagProvider;
        _tagWriter = tagWriter;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserTags()
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return BadRequest("Invalid user");
        }

        var tags = await _tagProvider.GetUserTagsAsync(userId.Value);

        return Ok(tags);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
    {
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            return BadRequest("Invalid user");
        }

        var userTags = await _tagProvider.GetUserTagsAsync(userId.Value);
        if (userTags.Any(t => t.TagName == request.TagName))
        {
            return BadRequest("Tag already exists with supplied tag name");
        }

        var tagCreated = await _tagWriter.CreateTagAsync(userId.Value, request.TagName);
        if (!tagCreated)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return Ok();
    }
}
