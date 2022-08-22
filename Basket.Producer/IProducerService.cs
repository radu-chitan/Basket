namespace Basket.Producer
{
    public interface IProducerService
    {
        Task<bool> SendOrderRequest(string topic, string message, string server);
    }
}