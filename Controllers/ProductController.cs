using Hazelcast;
using Microsoft.AspNetCore.Mvc;
using HazelcastCaching.Caching;
using HazelcastCaching.Models;
using HazelcastCaching.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("all")]
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAllProducts();
        }

        [HttpPost]
        public async Task PostProduct([FromBody] Product product)
        {
            await caching.WriteToCacheAsync(ServiceSettings.HazelcastCacheName, product.Code, product);

            await repository.AddProductAsync(product);
        }

        [HttpGet("{code}")]
        public async Task<Product> GetProduct(string code)
        {
            var product = await caching.ReadFromCacheAsync(ServiceSettings.HazelcastCacheName, code);

            if (product != null)
                return product;

            product = await repository.GetProductAsync(code);
            await caching.WriteToCacheAsync(ServiceSettings.HazelcastCacheName, code, product);

            return product;
        }

        [HttpDelete("{code}")]
        public async Task DeleteProduct(string code)
        {
            await caching.DeleteFromCacheAsync(ServiceSettings.HazelcastCacheName, code);

            await repository.DeleteProductAsync(code);
        }

        /*[HttpPut]
        public async Task UpdateProduct([FromBody] Product product)
        {
            await caching.WriteToCacheAsync(ServiceSettings.HazelcastCacheName, product.Code, product);

            await repository.UpdateProductAsync(code, product);
        }*/

        [HttpPut("increase-by-percent/{percent}/{code}")]
        public async Task IncreaseProductPrice(float percent, string code)
        {
            var product = await caching.ExecuteAsync(ServiceSettings.HazelcastCacheName, ChangePriceType.Increase, code, percent);

            await repository.UpdateProductAsync(code, product);
        }

        [HttpPut("decrease-by-percent/{percent}/{code}")]
        public async Task DecreaseProductPrice(float percent, string code)
        {
            var product = await caching.ExecuteAsync(ServiceSettings.HazelcastCacheName, ChangePriceType.Decrease, code, percent);

            await repository.UpdateProductAsync(code, product);
        }



        //--
        [HttpGet("test")]
        public async Task Test()
        {
            var map = await client.GetMapAsync<string, string>("processing-map");
            //await map.ExecuteAsync(new IncreasePriceEntryProcessor("processed"), "key");

            Console.WriteLine($"Value for key is: {await map.GetAsync("key")}");
        }
    }
}
