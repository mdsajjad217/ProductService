using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Option
{
    public class KafkaConsumerOptions
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public ConsumerOptions Consumer { get; set; } = new();
    }

    public class ConsumerOptions
    {
        public string GroupId { get; set; } = string.Empty;
        public string AutoOffsetReset { get; set; } = "Earliest";
        public bool EnableAutoCommit { get; set; }
    }
}
