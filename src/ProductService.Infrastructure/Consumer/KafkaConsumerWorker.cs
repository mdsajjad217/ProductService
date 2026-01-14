using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using ProductService.Domain.Event;
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

        public KafkaConsumerWorker()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "payment-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("order-created");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);

                try
                {
                    var evt = JsonSerializer.Deserialize<OrderCreatedEvent>(result.Message.Value);

                    //await ProcessEventAsync(evt);

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
