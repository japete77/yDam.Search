using MongoDB.Driver;

namespace yDevs.Dam.Services.Database
{
    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<T> GetCollection<T>();
        
    }
}