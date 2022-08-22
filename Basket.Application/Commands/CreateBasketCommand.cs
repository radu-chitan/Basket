using MediatR;

namespace Basket.Application.Commands
{
    public class CreateBasketCommand: IRequest
    {
        public string Customer { get; set; }

        public bool PaysVAT { get; set; }
    }
}
