using CustomerMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerMicroservice.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Customer> Customers {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Customer>()
                .Property(c => c.firstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.lastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Customer>()
                .Property(c => c.email)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
