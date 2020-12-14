using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace server.Models
{
  public abstract class Links
  {
    [BsonDefaultValue(null)]
    public string steam{ get; set; }
    [BsonDefaultValue(null)]
    public string oculus{ get; set; }
  }

  public class RedactedUser
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    [BsonRequired]
    public string username { get; set; }
    [BsonDefaultValue(false)]
    public bool verified{ get; set; }
    [BsonDefaultValue(false)]
    public bool admin{ get; set; }
    public Links links{ get; set; }
  }

  public class User : RedactedUser
  {
    [BsonRequired]
    public string email{ get; set; }
    [BsonRequired]
    public string password{ get; set; }

    [BsonDefaultValue(null)]
    public string verifyToken{ get; set; }
  }

}
