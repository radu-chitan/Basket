namespace Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int BasketId { get; set; }
        public Basket? Basket { get; set; }
    }
}
