﻿using System;
namespace BioTekno.Application.Features.Orders.Commands
{
	public class CreateOrderRequest
	{
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerGSM { get; set; }

        public List<ProductDetail> ProductDetails { get; set; }
    }

    public class ProductDetail
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }
    }
}

