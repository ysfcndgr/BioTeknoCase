using System;
using BioTekno.Domain.Base;
using BioTekno.Domain.Enums;

namespace BioTekno.Domain.Entities
{
	public sealed class Product : BaseEntity
	{
        public string Description { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductStatus Status { get; set; }
    }
}

