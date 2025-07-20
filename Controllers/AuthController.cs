using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using MusicTagger.Models.Requests;
using MusicTagger.Database.User;
using MusicTagger.Auth;
using Microsoft.AspNetCore.Authorization;

namespace MusicTagger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserProvider _userProvider;
    private readonly IUserWriter _userWriter;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthController(
        IUserProvider userProvider,
        IUserWriter userWriter,
        ITokenGenerator tokenGenerator)
    {
        _userProvider = userProvider;
        _userWriter = userWriter;
        _tokenGenerator = tokenGenerator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromForm] LoginRequest request)
    {
        var user = await _userProvider.GetUserAsync(request.Username);
        if (user == null) return BadRequest("No user found with that username");

        if (!BC.Verify(request.Password, user.PasswordHash)) return BadRequest("Wrong password");

        var token = _tokenGenerator.GenerateToken(user.Username, user.UserId);

        return Ok(new { token });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
    {
        var user = await _userProvider.GetUserAsync(request.Username);
        if (user != null) return BadRequest("A user already exists with that username");

        var passwordHash = BC.HashPassword(request.Password);
        var userId = await _userWriter.CreateUserAsync(request.Username, passwordHash);
        if (!userId.HasValue) return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create user");

        var token = _tokenGenerator.GenerateToken(request.Username, userId.Value);

        return Ok(new { token });
    }
}
