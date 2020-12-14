using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers
{
  [Route("users")]
  [ApiController]
  public class Users : ControllerBase
  {
    private readonly UserService _userService;

    public Users(UserService userService)
    {
      _userService = userService;
    }

    [HttpGet("me")]
    public ActionResult<RedactedUser> Me()
    {
      var user = _userService.GetByEmail("XD@XD.XD");

      if (user == null)
        return NotFound();

      return new RedactedUser
      {
        _id = user._id,
        admin = user.admin,
        links = user.links,
        username = user.username,
        verified = user.verified
      };
    }

    [HttpGet("find/{id}")]
    public ActionResult<RedactedUser> Find(string id)
    {
      var user = _userService.GetById(id);
      if (user == null)
        return NotFound();

      return new RedactedUser
      {
        _id = user._id,
        admin = user.admin,
        links = user.links,
        username = user.username,
        verified = user.verified
      };
    }
  }
}
