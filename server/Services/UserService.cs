using MongoDB.Driver;
using server.Models;

namespace server.Services
{
  public class UserService
  {
    private readonly IMongoCollection<User> _Users;

    public UserService(IDatabaseSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _Users = database.GetCollection<User>(settings.UserCollectionName);
    }

    public User GetByEmail(string email) =>
      _Users.Find(user => user.email == email).FirstOrDefault();
    public User GetById(string id) =>
      _Users.Find(user => user._id == id).FirstOrDefault();

    public User Create(User user)
    {
      _Users.InsertOne(user);
      return user;
    }

    public void Update(string id, User UserIn) =>
      _Users.ReplaceOne(User => User.email == id, UserIn);

    public void Remove(User UserIn) =>
      _Users.DeleteOne(User => User.email == UserIn.email);

    public void Remove(string id) =>
      _Users.DeleteOne(User => User.email == id);


  }
}
