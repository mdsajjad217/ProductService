using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProductService.Domain.Event;
using ProductService.Domain.Option;
using ProductService.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace ProductService.Infrastructure.Consumer
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public KafkaConsumerWorker(IOptions<KafkaConsumerOptions> options, IServiceScopeFactory scopeFactory)
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

            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var productCommandService = scope.ServiceProvider.GetRequiredService<IProductCommandService>();
            JsonSerializerOptions JsonOptions =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);

                try
                {
                    // First: deserialize string
                    var json = JsonSerializer.Deserialize<string>(
                        result.Message.Value,
                        JsonOptions
                    );

                    // Second: deserialize object
                    var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(
                        json!,
                        JsonOptions
                    );

                    await productCommandService.CreateOrderAsync(evt);

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
