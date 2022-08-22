namespace Domain.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public string Customer { get; set; } //IsRequired in BasketConfiguration
        public bool PaysVAT { get; set; }
        public ICollection<Item>? Items { get; set; }
    }
}