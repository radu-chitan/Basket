using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> options)
            : base(options) {}

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketDbContext).Assembly);
        }
    }
}
