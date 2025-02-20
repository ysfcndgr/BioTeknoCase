using System;
using BioTekno.Application.Dtos;
using MediatR;

namespace BioTekno.Application.Features.Orders.Commands
{
    public class CreateOrderCommand : IRequest<ApiResponse<int>>
    {
        public CreateOrderRequest OrderRequest { get; set; }

        public CreateOrderCommand(CreateOrderRequest orderRequest)
        {
            OrderRequest = orderRequest;
        }
    }
}

