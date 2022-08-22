using Basket.Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Basket.Application.Commands
{
    public class AddItemToBasketCommandHandler : IRequestHandler<AddItemToBasketCommand, Unit>
    {
        private readonly IBasketDbContext dbContext;

        public AddItemToBasketCommandHandler(IBasketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(AddItemToBasketCommand request, CancellationToken cancellationToken)
        {
            var item = new Domain.Entities.Item { BasketId = request.BasketId, Name = request.Item, Price = request.Price };
            if (item.Price < 0)
                throw new InvalidPriceException(item);

            await dbContext.Items.AddAsync(item, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
