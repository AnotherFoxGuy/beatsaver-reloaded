using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Models;
using WebApi.Helpers;
using server.Helpers;

namespace server.Services
{
  public class MapsService
  {
    private readonly IMongoCollection<Beatmap> _Beatmaps;
    private readonly AppSettings _appSettings;

    public MapsService(IOptions<DatabaseSettings> settings, IOptions<AppSettings> appSettings)
    {
      _appSettings = appSettings.Value;
      var client = new MongoClient(settings.Value.ConnectionString);
      var database = client.GetDatabase(settings.Value.DatabaseName);

      _Beatmaps = database.GetCollection<Beatmap>("beatmaps");
    }

    public Beatmap Create(Beatmap map)
    {
      _Beatmaps.InsertOne(map);
      return map;
    }

    public Beatmap GetBykey(string key) =>
      _Beatmaps.Find(Beatmap => Beatmap.key == key).FirstOrDefault();

    void Update(string id, Beatmap BeatmapIn) =>
      _Beatmaps.ReplaceOne(Beatmap => Beatmap.key == id, BeatmapIn);

    public void Remove(Beatmap BeatmapIn) =>
      _Beatmaps.DeleteOne(Beatmap => Beatmap.key == BeatmapIn.key);

    public void Remove(string id) =>
      _Beatmaps.DeleteOne(Beatmap => Beatmap.key == id);

    public List<Beatmap> Get()
    {
      return _Beatmaps.Find(x => true).ToList();
    }

    public async Task<Paginate<Beatmap>> Get(int page)
    {
      var maps = await _Beatmaps.AggregateByPage(Builders<Beatmap>.Sort.Ascending(x => x.name), page, 10);
      return new Paginate<Beatmap>
      {
        docs = maps.data.ToList(),
        totalPages = maps.totalPages
      };
    }

    public class Paginate<T>
    {
      public List<T> docs{ get; set; }
      public int totalDocs{ get; set; }
      public int totalPages{ get; set; }
      public int prevPage{ get; set; }
      public int nextPag{ get; set; }
    }
  }
}
