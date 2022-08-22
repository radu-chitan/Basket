using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

namespace Basket.UnitTests
{
    public class ContextFixture
    {
        public static BasketDbContext SetupContext()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<BasketDbContext>()
                .UseInMemoryDatabase($"Database{Guid.NewGuid()}")
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;

            var dbContext = new BasketDbContext(dbContextOptions);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        public static void SeedContext(BasketDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Baskets.Add(new Domain.Entities.Basket { Customer = "Test", PaysVAT = true });
            context.Items.Add(new Domain.Entities.Item { Name = "Test_item", BasketId = 1, Price = 10 });
            context.SaveChanges();
        }
    }
}
