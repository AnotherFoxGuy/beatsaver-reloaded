using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using server.Models;
using server.Services;

namespace server.Controllers
{
  [Route("auth")]
  [ApiController]
  public class Auth : ControllerBase
  {
    private readonly UserService _userService;

    public Auth(UserService userService)
    {
      _userService = userService;
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

      var salt = Convert.FromBase64String("NZsP6NnmfBuYeJrrAKNuVQ==");
      var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: user.password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA1,
        iterationCount: 10000,
        numBytesRequested: 256 / 8));
      user.password = hashed;
      _userService.Create(user);

      return NoContent();
    }

    [HttpPost("login")]
    public IActionResult Login(string username, string password)
    {
      Response.Cookies.Append("x-auth-token","cfsdfcsdc");

      return NoContent();
    }
  }
}
