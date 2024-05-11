using AuthenticationMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationMicroservice.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {  
            modelBuilder.Entity<User>()
                .HasIndex(x => x.userName)
                .IsUnique();
        }

    }
}
