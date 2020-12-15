// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using server.Models;

namespace server.Models
{
  public enum EVote
  {
    Upvote = 1,
    Downvote = -1
  }

  public class Vote
  {
    public EVote direction { get; set; }
    public string voterUID { get; set; }
  }

  public class Difficulties
  {
    public bool easy { get; set; }
    public bool expert { get; set; }
    public bool expertPlus { get; set; }
    public bool hard { get; set; }
    public bool normal { get; set; }
  }

  public class IParsedDifficulty
  {
    public double duration { get; set; }
    public int length { get; set; }
    public int njs { get; set; }
    public int njsOffset { get; set; }
    public int bombs { get; set; }
    public int notes { get; set; }
    public int obstacles { get; set; }
  }

  public class IDifficulties
  {
    public IParsedDifficulty easy { get; set; }
    public IParsedDifficulty expert { get; set; }
    public IParsedDifficulty expertPlus { get; set; }
    public IParsedDifficulty hard { get; set; }
    public IParsedDifficulty normal { get; set; }
  }

  public class IBeatmapCharacteristic
  {
    public IDifficulties difficulties { get; set; }
    public string name { get; set; }
  }

  public class Metadata
  {
    public Difficulties difficulties { get; set; }
    public int duration { get; set; }
    public List<IBeatmapCharacteristic> characteristics { get; set; }
    public string levelAuthorName { get; set; }
    public string songAuthorName { get; set; }
    public string songName { get; set; }
    public string songSubName { get; set; }
    public int bpm { get; set; }
  }

  public class Stats
  {
    public int downloads { get; set; }
    public int plays { get; set; }
    public int downVotes { get; set; }
    public int upVotes { get; set; }
    public double heat { get; set; }
    public int rating { get; set; }
  }

  public class Beatmap
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }

    public Metadata metadata { get; set; }
    public Stats stats { get; set; }
    public string description { get; set; }
    public object deletedAt { get; set; }

    public string name { get; set; }

    //[BsonRepresentation(BsonType.ObjectId)]
    //public string uploader { get; set; }

    [BsonSerializer(typeof(uploaderSerializer))]
    public MongoDBRef uploader { get; set; }

    public string hash { get; set; }
    public string coverExt { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime uploaded { get; set; }

    public List<Vote> votes { get; set; }
    [BsonIgnore] public string directDownload => $"/cdn/{key}/{hash}.zip";

    [BsonIgnore] public string downloadURL => $"/download/key/{key}";

    [BsonIgnore] public string coverURL => $"/cdn/{key}/{hash}{coverExt}";

    [BsonSerializer(typeof(HexSerializer))]
    public string key { get; set; }

    public int __v { get; set; }
  }


  class uploaderSerializer : SerializerBase<MongoDBRef>
  {
    public override MongoDBRef Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
      return new MongoDBRef("users", context.Reader.ReadObjectId());
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, MongoDBRef value)
    {
      //context.Writer.WriteInt32(Int32.Parse(value, System.Globalization.NumberStyles.HexNumber));
    }
  }class HexSerializer : SerializerBase<string>
  {
    public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
      return Convert.ToString(context.Reader.ReadInt32(), 16);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
    {
      context.Writer.WriteInt32(Int32.Parse(value, System.Globalization.NumberStyles.HexNumber));
    }
  }
}
