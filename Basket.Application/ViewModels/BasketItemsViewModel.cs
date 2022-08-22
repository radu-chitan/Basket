namespace Basket.Application.ViewModels
{
    public class BasketItemsViewModel
    {
        public int Id { get; set; }
        public string? Customer { get; set; }
        public bool PaysVAT { get; set; }
        public double TotalNET { get; set; }
        public double TotalGross { get; set; }
        public IEnumerable<ItemViewModel>? Items { get; set; }
    }
}
