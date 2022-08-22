using Basket.Application.Commands;
using Basket.Application.Interfaces;
using Confluent.Kafka;
using System.Text.Json;

namespace Basket.Consumer.Services
{
    public class CheckoutBasketConsumerService : IHostedService
    {
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;

        public CheckoutBasketConsumerService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            this.configuration = configuration;
            this.scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = Domain.Common.Constants.BASKET_GROUP_ID,
                BootstrapServers = configuration.GetSection("Settings").GetValue<string>("BootstrapServer"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
                consumerBuilder.Subscribe(Domain.Common.Constants.BASKET_CHECKOUT);
                var cancelToken = new CancellationTokenSource();
                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);
                    var checkoutBasketCommand = JsonSerializer.Deserialize<CheckoutBasketCommand>(consumer.Message.Value);
                    if (checkoutBasketCommand != null)
                    {
                        using var scope = scopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<IBasketDbContext>();
                        var basket = dbContext.Baskets.Find(checkoutBasketCommand.BasketId);
                        if (basket != null)
                        {
                            dbContext.Items.RemoveRange(dbContext.Items.Where(t => t.BasketId == checkoutBasketCommand.BasketId));
                            dbContext.Baskets.Remove(basket);
                            await dbContext.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
