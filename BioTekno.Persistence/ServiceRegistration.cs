using System;
using BioTekno.Application.Interfaces;
using BioTekno.Persistence.Context;
using BioTekno.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioTekno.Persistence
{
	public static class ServiceRegistration
	{
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<BioTeknoDbContext>(opt =>
            {
                opt.UseMySql(connectionString, serverVersion,
                 options => options.MigrationsAssembly("BioTekno.Persistence"));
            });
        }

    }
}