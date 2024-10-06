using Bookstore.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Persistence.Contexts;

public class MongoUsersDbContext : IdentityDbContext<User, UserRole, Guid>
{
    public DbSet<User> Users { get; set; }

    public MongoUsersDbContext(DbContextOptions<MongoUsersDbContext> options) :
        base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<User>();
    }
}