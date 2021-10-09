using MongoDB.Driver;
using HazelcastCaching.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HazelcastCaching.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoClient client;

        public ProductRepository(IMongoClient client)
        {
            this.client = client;
        }

        public async Task AddProductAsync(Product product)
        {
            var db = client.GetDatabase(ServiceSettings.MongoDatabaseName);
            var collection = db.GetCollection<Product>(ServiceSettings.MongoCollectionName);
            
            await collection.InsertOneAsync(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var db = client.GetDatabase(ServiceSettings.MongoDatabaseName);
            var collection = db.GetCollection<Product>(ServiceSettings.MongoCollectionName);

            return collection.Find(FilterDefinition<Product>.Empty).ToEnumerable();
        }

        public async Task<Product> GetProductAsync(string code)
        {
            var db = client.GetDatabase(ServiceSettings.MongoDatabaseName);
            var collection = db.GetCollection<Product>(ServiceSettings.MongoCollectionName);

            return await collection.Find(Builders<Product>.Filter.Eq(x => x.Code, code)).FirstOrDefaultAsync();
        }
    }
}
