using System;
using BioTekno.Domain.Entities;
using BioTekno.Persistence.Context;

namespace BioTekno.Persistence
{

    public static class DbInitializer
    {
        public static void Initialize(BioTeknoDbContext context)
        {
            context.Database.EnsureCreated(); 

            if (context.Products.Any())
            {
                return;
            }

            var products = new List<Product>
        {
            new Product { Description = "Product 1", Category = "Electronics", Unit = "Piece", UnitPrice = 100, Status = Domain.Enums.ProductStatus.Success, CreatedDate = DateTime.UtcNow },
            new Product { Description = "Product 2", Category = "Clothing", Unit = "Piece", UnitPrice = 50, Status = Domain.Enums.ProductStatus.Failed, CreatedDate = DateTime.UtcNow },
            new Product { Description = "Product 3", Category = "Home", Unit = "Piece", UnitPrice = 200, Status = Domain.Enums.ProductStatus.Failed, CreatedDate = DateTime.UtcNow },
        };

            for (int i = 4; i <= 1000; i++)
            {
                products.Add(new Product
                {
                    Description = $"Product {i}",
                    Category = i % 2 == 0 ? "Electronics" : "Clothing",
                    Unit = "Piece",
                    UnitPrice = new Random().Next(10, 500),
                    Status = Domain.Enums.ProductStatus.Success,
                    CreatedDate = DateTime.UtcNow
                });
            }

            context.Products.AddRange(products);
            context.SaveChanges();

            var orders = new List<Order>
        {
            new Order
            {
                CustomerName = "John Doe",
                CustomerEmail = "john.doe@example.com",
                CustomerGSM = "5551234567",
                TotalAmount = 300,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { ProductId = 1, UnitPrice = 100, Amount = 1 },
                    new OrderDetail { ProductId = 2, UnitPrice = 50, Amount = 2 }
                }
            }
        };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}

