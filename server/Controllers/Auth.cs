using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using server.Models;
using server.Services;
using WebApi.Helpers;

namespace server.Controllers
{
  [Route("auth")]
  [ApiController]
  public class Auth : ControllerBase
  {
    private readonly UserService _userService;
    private readonly AppSettings _appSettings;

    public Auth(UserService userService, IOptions<AppSettings> appSettings)
    {
      _userService = userService;
      _appSettings = appSettings.Value;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
      if (user.username == null)
        return BadRequest("Username not set!");
      if (user.email == null)
        return BadRequest("Email not set!");
      if (user.password == null)
        return BadRequest("Password not set!");

      user.password = hashPassword(user.password);
      _userService.Create(user);

      return NoContent();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User u)
    {
      var user = _userService.GetByUsername(u.username);
      if (user == null)
        return NotFound();
      var pass = hashPassword(u.password);
      if(pass != user.password)
        return Unauthorized();

      var token = _userService.GenerateJwtToken(user);

      Response.Headers.Add("x-auth-token", token);

      return NoContent();
    }

    private string hashPassword(string pass)
    {
      var salt = Convert.FromBase64String(_appSettings.Salt);
      return Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: pass,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA1,
        iterationCount: 10000,
        numBytesRequested: 256 / 8));
    }
  }
}
