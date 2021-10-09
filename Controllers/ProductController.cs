using Hazelcast;
using Microsoft.AspNetCore.Mvc;
using HazelcastCaching.Caching;
using HazelcastCaching.Models;
using HazelcastCaching.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HazelcastCaching.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository repository;
        private IHazelcastCaching<string, Product> caching;

        public ProductController(IProductRepository repository,
                                 IHazelcastClient hazelcastClient,
                                 IHazelcastCaching<string, Product> caching)
        {
            this.repository = repository;
            this.caching = caching;
        }

        [HttpGet("all")]
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAllProducts();
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            await caching.WriteToCacheAsync(ServiceSettings.HazelcastCacheName, product.Code, product);

            await repository.AddProductAsync(product);

            return Ok();
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
    }
}
