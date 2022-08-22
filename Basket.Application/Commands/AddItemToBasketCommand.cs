namespace Basket.Application.Commands
{
    public class AddItemToBasketCommand
    {
        public string? Item { get; set; }
        public int Price { get; set; }
        public int BasketId { get; set; }
    }
}
