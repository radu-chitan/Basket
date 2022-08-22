using Basket.Application.Commands;
using Basket.Application.Interfaces;
using Confluent.Kafka;
using System.Text.Json;

namespace Basket.Consumer.Services
{
    public class BasketCreationConsumerService : IHostedService
    {
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;

        public BasketCreationConsumerService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
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
                consumerBuilder.Subscribe(Domain.Common.Constants.BASKET_CREATE_TOPIC);
                var cancelToken = new CancellationTokenSource();
                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);
                    var createBasketRequest = JsonSerializer.Deserialize<CreateBasketCommand>(consumer.Message.Value);
                    //Command handling starts here
                    //Could use mediatr to pass to a handler
                    if (createBasketRequest != null)
                    {
                        using var scope = scopeFactory.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<IBasketDbContext>();
                        await dbContext.Baskets.AddAsync(new Domain.Entities.Basket { Customer = createBasketRequest.Customer, PaysVAT = createBasketRequest.PaysVAT }, cancellationToken);
                        await dbContext.SaveChangesAsync(cancellationToken);
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