using BioTekno.Application.Interfaces;
using BioTekno.Domain.Entities;
using BioTekno.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage;

namespace BioTekno.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly StackExchange.Redis.IDatabase _redisDb;

        public ProductRepository(BioTeknoDbContext context, IConnectionMultiplexer redisConnection)
            : base(context)
        {
            _redisDb = redisConnection.GetDatabase();
        }

        public async Task<List<Product>> GetProductsWithCacheAsync(string category)
        {
            string cacheKey = string.IsNullOrEmpty(category) ? "all_products" : $"products_{category}";

            var cachedData = await _redisDb.StringGetAsync(cacheKey);
            if (cachedData.HasValue)
            {
               return JsonConvert.DeserializeObject<List<Product>>(cachedData);
            }

            IQueryable<Product> query = _dbSet;
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            var products = await query.ToListAsync();

            var serialized = JsonConvert.SerializeObject(products);
            await _redisDb.StringSetAsync(cacheKey, serialized, TimeSpan.FromMinutes(10));

            return products;
        }
    }
}
