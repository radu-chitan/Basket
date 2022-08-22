using MediatR;

namespace Basket.Application.Commands
{
    public class AddItemToBasketCommand: IRequest
    {
        public string? Item { get; set; }
        public int Price { get; set; }
        public int BasketId { get; set; }
    }
}
