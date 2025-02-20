using BioTekno.Application.Interfaces;
using BioTekno.Persistence.Context;

namespace BioTekno.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BioTeknoDbContext _context;
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(BioTeknoDbContext context, IProductRepository productRepo, IOrderRepository orderRepo)
        {
            _context = context;
            Products = productRepo;
            Orders = orderRepo;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
