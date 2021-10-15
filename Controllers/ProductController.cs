using Hazelcast;
using Microsoft.AspNetCore.Mvc;
using HazelcastCaching.Caching;
using HazelcastCaching.Models;
using HazelcastCaching.Repository;
using System.Threading.Tasks;
using HazelcastCaching.EntryProcessors;

namespace HazelcastCaching.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository repository;
        private IHazelcastCaching<string, Product> caching;
        private IHazelcastClient client;

        public ProductController(IProductRepository repository,
                                 IHazelcastClient hazelcastClient,
                                 IHazelcastCaching<string, Product> caching)
        {
            this.repository = repository;
            this.caching = caching;
            client = hazelcastClient;
        }

        [HttpPost]
        public async Task PostProduct([FromBody] Product product)
        {
            var cacheTask = caching.WriteToCacheAsync(ServiceSettings.HazelcastCacheName, product.Code, product);

            var dbTask = repository.AddProductAsync(product);

            await Task.WhenAll(cacheTask, dbTask);
        }

        [HttpGet("{code}")]
        public async Task<Product> GetProduct(string code)
        {
            var product = await caching.ReadFromCacheAsync(ServiceSettings.HazelcastCacheName, code);

            if (product != null)
                return product;

            product = await repository.GetProductAsync(code);

            if(product != null)
                await caching.WriteToCacheAsync(ServiceSettings.HazelcastCacheName, code, product);

            return product;
        }

        [HttpDelete("{code}")]
        public async Task DeleteProduct(string code)
        {
            var cacheTask =  caching.DeleteFromCacheAsync(ServiceSettings.HazelcastCacheName, code);

            var dbTask = repository.DeleteProductAsync(code);

            await Task.WhenAll(cacheTask, dbTask);
        }

        [HttpPut("increase-by-percent/{percent}/{code}")]
        public async Task IncreaseProductPrice(float percent, string code)
        {
            var map = await client.GetMapAsync<string, Product>(ServiceSettings.HazelcastCacheName);

            if (!await map.ContainsKeyAsync(code))
            {
                await map.PutAsync(code, await repository.GetProductAsync(code));
            }

            var product = await caching.ExecuteAsync(ServiceSettings.HazelcastCacheName, ChangePriceType.Increase, code, percent);

            await repository.UpdateProductAsync(code, product);
        }

        [HttpPut("decrease-by-percent/{percent}/{code}")]
        public async Task DecreaseProductPrice(float percent, string code)
        {
            var map = await client.GetMapAsync<string, Product>(ServiceSettings.HazelcastCacheName);

            if(!await map.ContainsKeyAsync(code))
            {
                await map.PutAsync(code, await repository.GetProductAsync(code));
            }

            var product = await caching.ExecuteAsync(ServiceSettings.HazelcastCacheName, ChangePriceType.Decrease, code, percent);

            await repository.UpdateProductAsync(code, product);
        }
    }
}
