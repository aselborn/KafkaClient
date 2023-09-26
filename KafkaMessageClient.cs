using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkQueue.topic.v1;

using workQueueMessage = WorkQueue.topic.v1.WorkQueue;

namespace kafkaclient
{
    public class KafkaMessageClient : KafkaBaseConfig
    {
        public KafkaMessageClient() { }

        public void SendMessage(workQueueMessage message)
        {
          

            using var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);
            using var producer =
                new ProducerBuilder<string, WorkQueue.topic.v1.WorkQueue>(config)
                .SetValueSerializer(new AvroSerializer<WorkQueue.topic.v1.WorkQueue>(schemaRegistry).AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error : {e.Reason}"))
                .Build();

            

            producer.Produce(_topicName, new Message<string, WorkQueue.topic.v1.WorkQueue>
            {
                Key = "text",
                Value = message
            }, report =>
            {
                processSuccess = report.Error.Code == ErrorCode.NoError;
                Console.WriteLine("Meddelandet skickades!");
            });

            producer.Flush();
        }

    }
}
