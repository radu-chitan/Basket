using Basket.Application.Interfaces;
using MediatR;

namespace Basket.Application.Commands
{
    public class CheckoutBasketCommandHandler: IRequestHandler<CheckoutBasketCommand, Unit>
    {
        private readonly IBasketDbContext dbContext;

        public CheckoutBasketCommandHandler(IBasketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = dbContext.Baskets.Find(request.BasketId);
            if (basket != null)
            {
                dbContext.Items.RemoveRange(dbContext.Items.Where(t => t.BasketId == request.BasketId));
                dbContext.Baskets.Remove(basket);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
