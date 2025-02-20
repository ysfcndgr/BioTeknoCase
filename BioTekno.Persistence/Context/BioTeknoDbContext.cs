using System;
using BioTekno.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BioTekno.Persistence.Context
{
	public class BioTeknoDbContext : DbContext
	{
        public BioTeknoDbContext(DbContextOptions<BioTeknoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
                entity.Property(p => p.Category).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Unit).IsRequired().HasMaxLength(50);
                entity.Property(p => p.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(p => p.Status).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired(); // Added for base property
                entity.Property(p => p.UpdateDate); // Nullable
            });

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(100);
                entity.Property(o => o.CustomerGSM).IsRequired().HasMaxLength(15);
                entity.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(o => o.CreatedDate).IsRequired(); // Added for base property
                entity.Property(o => o.UpdateDate); // Nullable
                entity.HasMany(o => o.OrderDetails)
                      .WithOne(od => od.Order)
                      .HasForeignKey(od => od.OrderId);
            });

            // OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetails");
                entity.HasKey(od => od.Id);
                entity.Property(od => od.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(od => od.Amount).IsRequired();
                entity.HasOne(od => od.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderId);
                entity.HasOne(od => od.Product)
                      .WithMany()
                      .HasForeignKey(od => od.ProductId);
            });
        }




    }
}
