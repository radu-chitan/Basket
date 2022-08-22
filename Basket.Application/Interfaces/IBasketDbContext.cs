using Microsoft.EntityFrameworkCore;

namespace Basket.Application.Interfaces
{
    public interface IBasketDbContext
    {
        DbSet<Domain.Entities.Basket> Baskets { get; set; }

        DbSet<Domain.Entities.Item> Items { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
