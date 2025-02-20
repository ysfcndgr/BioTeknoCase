using System;
using BioTekno.Domain.Base;

namespace BioTekno.Domain.Entities
{
    public sealed class Order : BaseEntity
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerGSM { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

