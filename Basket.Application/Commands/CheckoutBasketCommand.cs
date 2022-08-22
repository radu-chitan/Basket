using MediatR;

namespace Basket.Application.Commands
{
    public class CheckoutBasketCommand: IRequest
    {
        public int BasketId { get; set; }
    }
}
