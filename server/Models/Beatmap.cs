using System;
using System.Reflection.Metadata;

namespace server.Models
{
  internal enum EVote
  {
    Upvote = 1,
    Downvote = -1
  }

  internal abstract class EDifficulties
  {
    public bool easy;
    public bool normal;
    public bool hard;
    public bool expert;
    public bool expertPlus;
  }

  internal abstract class Metadata
  {
    public string songName;

    public string songSubName;

    public string songAuthorName;

    public string levelAuthorName;

    public int duration;

    public int bpm;

    public IBeatmapCharacteristic[] characteristics;
  }

  internal abstract class Stats
  {
    public int downloads;
    public int plays;
    public int upVotes;
    public int downVotes;
    public int rating;
    public int heat;
  }

  internal abstract class Vote
  {
    public EVote direction;
    public string voterUID;
  }

  internal abstract class Beatmap
  {
    public string key;
    public string name;
    public string description;
    public User uploader;
    public DateTime uploaded;
    public DateTime deletedAt;

    public Metadata metadata;
    public Stats stats;
    public Vote[] votes;

    public string directDownload;

    public string downloadURL;

    public string coverURL;

    public string coverExt;

    public string hash;
  }

  public abstract class IParsedDifficulty
  {
    public int duration;
    public int length;
    public int bombs;
    public int notes;
    public int obstacles;
    public int njs;
    public int njsOffset;
  }

  public abstract class IDifficulties
  {
    public IParsedDifficulty easy;
    public IParsedDifficulty normal;
    public IParsedDifficulty hard;
    public IParsedDifficulty expert;
    public IParsedDifficulty expertPlus;
  }

  public abstract class IBeatmapCharacteristic
  {
    public string name;
    public IDifficulties difficulties;
  }
}
