using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProductService.Application.Option;
using ProductService.Domain.Event;
using ProductService.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Consumer
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IProductCommandService _productCommandService;

        public KafkaConsumerWorker(IOptions<KafkaConsumerOptions> options, IProductCommandService productCommandService)
        {
            var kafka = options.Value;

            var config = new ConsumerConfig
            {
                BootstrapServers = kafka.BootstrapServers,
                GroupId = kafka.Consumer.GroupId,
                EnableAutoCommit = kafka.Consumer.EnableAutoCommit,
                AutoOffsetReset = Enum.Parse<AutoOffsetReset>(
                    kafka.Consumer.AutoOffsetReset,
                    ignoreCase: true)
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("order-created");

            _productCommandService = productCommandService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);

                try
                {
                    var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(result.Message.Value);

                    await _productCommandService.CreateOrderAsync(evt);

                    _consumer.Commit(result);
                }
                catch (Exception ex)
                {
                    // DO NOT COMMIT
                    // Retry or send to DLT
                }
            }
        }
    }
}
