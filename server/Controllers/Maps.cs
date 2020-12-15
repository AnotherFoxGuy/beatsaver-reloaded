using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers
{
  [Route("maps")]
  [ApiController]
  public class Maps : ControllerBase
  {
    private readonly MapsService _mapsService;

    public Maps(MapsService mapsService)
    {
      _mapsService = mapsService;
    }

    [HttpGet("all")]
    public ActionResult<List<Beatmap>> Get() => _mapsService.Get();

    [HttpGet("detail/{key}")]
    public ActionResult<Beatmap> Detail(string key)
    {
      var map = _mapsService.GetBykey(key);
      if (map == null)
        return NotFound();

      return map;

    }
    [HttpGet("hot/{page}")]
    public async Task<MapsService.Paginate<Beatmap>> Hot(int page)
    {
      return await _mapsService.Get(page);
    }
  }
}
