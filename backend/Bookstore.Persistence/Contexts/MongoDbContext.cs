using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Persistence.Contexts;

public class MongoDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }

    public MongoDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>();
        modelBuilder.Entity<Review>();
    }
}