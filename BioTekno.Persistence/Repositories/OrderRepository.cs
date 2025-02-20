using BioTekno.Application.Interfaces;
using BioTekno.Domain.Entities;
using BioTekno.Persistence.Context;

namespace BioTekno.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(BioTeknoDbContext context) : base(context)
        {
        }
    }
}
