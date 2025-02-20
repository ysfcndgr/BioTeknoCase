using System;
using AutoMapper;
using BioTekno.Application.Dtos;
using BioTekno.Application.Features.Orders.Commands;
using BioTekno.Domain.Entities;

namespace BioTekno.Application.Mappings
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<Product, ProductDto>();
        }
    }
}

