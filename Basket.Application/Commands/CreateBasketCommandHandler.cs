using Basket.Application.Interfaces;
using MediatR;

namespace Basket.Application.Commands
{
    internal class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Unit>
    {
        private readonly IBasketDbContext dbContext;

        public CreateBasketCommandHandler(IBasketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            await dbContext.Baskets.AddAsync(new Domain.Entities.Basket { Customer = request.Customer, PaysVAT = request.PaysVAT }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
