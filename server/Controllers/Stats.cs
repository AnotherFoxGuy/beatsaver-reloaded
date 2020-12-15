using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers
{
  [Route("stats")]
  [ApiController]
  public class Stats : ControllerBase
  {
    private readonly MapsService _mapsService;

    public Stats(MapsService mapsService)
    {
      _mapsService = mapsService;
    }

    [HttpGet("key/{key}")]
    public ActionResult<Beatmap> ByKey(string key)
    {
      var map = _mapsService.GetBykey(key);
      if (map == null)
        return NotFound();

      return map;

    }
  }
}
