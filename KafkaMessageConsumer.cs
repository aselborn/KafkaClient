using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkQueue.topic.v1;

namespace kafkaclient
{
    public class KafkaMessageConsumer : KafkaBaseConfig
    {
        public KafkaMessageConsumer() { }

        public Task Run(CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                using var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);
                using var consumer =
                    new ConsumerBuilder<string, WorkQueue.topic.v1.WorkQueue>(_consumerConfig)
                    .SetValueDeserializer(new AvroDeserializer<WorkQueue.topic.v1.WorkQueue>(schemaRegistry).AsSyncOverAsync())
                    .SetErrorHandler((_, e) => Console.WriteLine(e.ToString()))
                    .Build();

                consumer.Subscribe(_topicName);
                Console.WriteLine($"Prenumererar på {_topicName}");

                ConsumeMessage(token, consumer);



            }, token);
        }

        public void Read()
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe(_topicName);
            while(true)
            {
                var consumeResult = consumer.Consume();
                Console.WriteLine(consumeResult);
            }
        }

        private void ConsumeMessage(CancellationToken token, IConsumer<string, WorkQueue.topic.v1.WorkQueue> consumer)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(token);

                    Console.WriteLine($"Consumed message {consumeResult.Message.Value.vesselName} on topic {consumeResult.Message.Key}",
                        consumeResult.Message, consumeResult.Topic);

                    var data = JsonSerializer.Serialize(consumeResult.Message);

                    Console.WriteLine(data);

                    //EntityReceived?.Invoke(this, new CustomEventArgs<WorkQueue.topic.v1.WorkQueue>(consumeResult.Message.Value));
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine("Consume error occurred: {ErrorReason}, {Record}", e.Error.Reason, JsonSerializer.Serialize(e.ConsumerRecord));
                    Console.WriteLine("Inner: {InnerException}", e.InnerException?.ToString());
                }
            }
        }

    }
}
