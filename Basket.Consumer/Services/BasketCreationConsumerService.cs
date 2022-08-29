﻿using Basket.Application.Commands;
using Basket.Application.Interfaces;
using Confluent.Kafka;
using MediatR;
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
                consumerBuilder.Subscribe(Domain.Common.Constants.BASKET_CREATE_TOPIC);
                var cancelToken = new CancellationTokenSource();
                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);
                    var createBasketRequest = JsonSerializer.Deserialize<CreateBasketCommand>(consumer.Message.Value);
                    if (createBasketRequest != null)
                    {
                        using var scope = scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetService<IMediator>();
                        await mediator.Send(createBasketRequest, cancellationToken);
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