using Basket.Application.Interfaces;
using Basket.Application.ViewModels;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Basket.Application.Queries
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, BasketItemsViewModel>
    {
        private readonly IBasketDbContext dbContext;

        public GetBasketItemsQueryHandler(IBasketDbContext basketDbContext)
        {
            dbContext = basketDbContext;
        }

        public async Task<BasketItemsViewModel> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
        {
            var basket = await dbContext.Baskets.Where(t => t.Id == request.BasketId)
                .Include(ctx => ctx.Items)
                .Select(basket => new BasketItemsViewModel
                {
                    Id = request.BasketId,
                    Customer = basket.Customer,
                    PaysVAT = basket.PaysVAT,
                    Items = basket.Items != null ? basket.Items.Select(t => new ItemViewModel { Item = t.Name, Price = t.Price}) : null,
                    TotalNET = 0,
                    TotalGross = 0
                })
                .SingleOrDefaultAsync();


            if (basket != null)
            {
                if (basket.Items != null)
                {
                    basket.TotalNET = basket.Items.Sum(item => item.Price);
                    basket.TotalGross = basket.PaysVAT ? 
                        basket.TotalNET + basket.TotalNET * Constants.VAT 
                        : basket.TotalNET;
                }
                return basket;
            }
            else
            {
                throw new ArgumentException($"Basket with id {request.BasketId} was not found!");
            }
        }
    }
}
