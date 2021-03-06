using System.Threading.Tasks;
using MongoDB.Driver;

namespace Common.Persistence.Mongo.Factories
{
    internal sealed class MongoSessionFactory : IMongoSessionFactory
    {
        private readonly IMongoClient _client;

        public MongoSessionFactory(IMongoClient client)
        {
            _client = client;
        }

        public Task<IClientSessionHandle> CreateAsync()
        {
            return _client.StartSessionAsync();
        }
    }
}