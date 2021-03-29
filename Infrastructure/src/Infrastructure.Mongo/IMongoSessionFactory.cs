using System.Threading.Tasks;
using MongoDB.Driver;

namespace Infrastructure.Mongo
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}