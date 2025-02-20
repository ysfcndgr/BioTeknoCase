using System;
using System.Reflection;
using AutoMapper;
using BioTekno.Application.Mappings;
using BioTekno.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BioTekno.Application
{
	public static class ServiceRegistration
	{
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}


