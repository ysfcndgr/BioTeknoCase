using BioTekno.Domain.Entities;

namespace BioTekno.Application.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsWithCacheAsync(string category);
    }
}
