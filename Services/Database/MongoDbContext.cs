using Microsoft.Extensions.Options;
using MongoDB.Driver;
using yDevs.Config;

namespace yDevs.Dam.Services.Database
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly AppSettings _settings;
        private IMongoDatabase _database;
        public MongoDbContext(IOptions<AppSettings> settings)
        {
            this._settings = settings.Value;
        }

        public IMongoDatabase Database { 
            get 
            {
                if (_database == null)
                {
                    _database = RegisterDatabase();
                }

                return _database;
            }
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            // use generic class name for the db collection name in mongoDb
            return Database.GetCollection<T>(typeof(T).Name);
        }

        private IMongoDatabase RegisterDatabase()
        {
            MongoClient client = new MongoClient(this._settings.Mongo.Url);
            return client.GetDatabase(this._settings.Mongo.Database);
        }
    }
}