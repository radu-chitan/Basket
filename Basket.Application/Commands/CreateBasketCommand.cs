namespace Basket.Application.Commands
{
    public class CreateBasketCommand
    {
        public string Customer { get; set; }

        public bool PaysVAT { get; set; }
    }
}
