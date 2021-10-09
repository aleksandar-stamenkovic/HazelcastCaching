using HazelcastCaching.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HazelcastCaching.Repository
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        IEnumerable<Product> GetAllProducts();
        Task<Product> GetProductAsync(string code);
    }
}
