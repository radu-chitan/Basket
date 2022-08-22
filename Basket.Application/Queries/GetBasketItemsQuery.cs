using Basket.Application.ViewModels;
using MediatR;

namespace Basket.Application.Queries
{
    public class GetBasketItemsQuery: IRequest<BasketItemsViewModel>
    {
        public int BasketId { get; set; }
    }
}
