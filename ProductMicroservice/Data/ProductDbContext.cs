using ProductMicroservice.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductMicroservice.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Product>()
                .Property(c => c.name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(c => c.description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Product>()
                .Property(c => c.price)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}

