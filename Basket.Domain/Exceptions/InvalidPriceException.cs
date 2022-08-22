using Domain.Entities;

namespace Domain.Exceptions
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException(Item item) 
            : base($"Invalid price detected for item: {item.Name} - {item.Price}. Amount cannot be negative!")
        { }
    }
}
