using Basket.Application.Commands;
using Confluent.Kafka;
using MediatR;
using System.Text.Json;

namespace Basket.Consumer.Services
{
    public class AddItemToBasketConsumerService : IHostedService
    {
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;

        public AddItemToBasketConsumerService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            this.configuration = configuration;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
           return Task.Run(async () => await Work(cancellationToken), cancellationToken);
        }

        private async Task Work(CancellationToken cancellationToken)
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
                consumerBuilder.Subscribe(Domain.Common.Constants.BASKET_ADD_ITEM_TOPIC);
                var cancelToken = new CancellationTokenSource();
                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);
                    var addItemCommand = JsonSerializer.Deserialize<AddItemToBasketCommand>(consumer.Message.Value);
                    if (addItemCommand != null)
                    {
                        using var scope = scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetService<IMediator>();
                        await mediator.Send(addItemCommand, cancellationToken);
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
