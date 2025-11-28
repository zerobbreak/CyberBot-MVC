using ChatBot.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatBot.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDb");
            var databaseName = config.GetSection("MongoDbSettings:Database").Value;
            
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
