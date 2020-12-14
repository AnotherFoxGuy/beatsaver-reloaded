using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using server.Models;
using WebApi.Helpers;

namespace server.Services
{

  public class UserService
  {
    private readonly IMongoCollection<User> _Users;
    private readonly AppSettings _appSettings;

    public UserService(IOptions<DatabaseSettings> settings, IOptions<AppSettings> appSettings)
    {
      _appSettings = appSettings.Value;
      var client = new MongoClient(settings.Value.ConnectionString);
      var database = client.GetDatabase(settings.Value.DatabaseName);

      _Users = database.GetCollection<User>("users");
    }

    public User GetByEmail(string email) =>
      _Users.Find(User => User.email == email).FirstOrDefault();
    public User GetByUsername(string username) =>
      _Users.Find(User => User.username == username).FirstOrDefault();
    public User GetById(string id) =>
      _Users.Find(User => User._id == id).FirstOrDefault();

    public User Create(User user)
    {
      _Users.InsertOne(user);
      return user;
    }

    public void Update(string id, User UserIn) =>
      _Users.ReplaceOne(User => User._id == id, UserIn);

    public void Remove(User UserIn) =>
      _Users.DeleteOne(User => User._id == UserIn._id);

    public void Remove(string id) =>
      _Users.DeleteOne(User => User._id == id);

    public string GenerateJwtToken(User user)
    {
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user._id) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }

  }
}
