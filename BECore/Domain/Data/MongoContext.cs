using Common.Helpers;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Domain.Data
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly IConfiguration _configuration;

        public MongoContext(IConfiguration configuration)
        {
            _configuration=configuration;

            var serverMongo = _configuration.GetSection(Constants.AuthConfig.MongoConnection + Constants.AuthConfig.ServerName).Value;
            var database = _configuration.GetSection(Constants.AuthConfig.MongoConnection + Constants.AuthConfig.Database).Value;

            var client = new MongoClient(serverMongo);
            if (client != null)
                _database = client.GetDatabase(database);
        }

        public IMongoDatabase Database
        {
            get
            {
                return _database;
            }
        }
    }

}
