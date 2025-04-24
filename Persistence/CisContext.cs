using Domain.Entities;
using MongoDB.Driver;



namespace Persistence;

public class CisContext 
{
      private readonly IMongoDatabase _database;

        public CisContext(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("Cis"); 
        }

        public IMongoCollection<Topic> Topics => _database.GetCollection<Topic>("Topics");
        public IMongoCollection<Idea> Ideas => _database.GetCollection<Idea>("Ideas");
        public IMongoCollection<Vote> Votes => _database.GetCollection<Vote>("Votes");

}