using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafkaclient
{
    public abstract class KafkaBaseConfig
    {
        protected SchemaRegistryConfig _schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "http://localhost:8081",
            EnableSslCertificateVerification = false
        };

        protected AvroSerializerConfig _avroSerializerConfig = new AvroSerializerConfig
        {
            // optional Avro serializer properties:
            BufferBytes = 100
        };

        protected ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        protected ConsumerConfig _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId="my-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        protected bool processSuccess = false;
        protected string _topicName = "mitt-topic";

        protected event EventHandler<CustomEventArgs<WorkQueue.topic.v1.WorkQueue>>? EntityReceived;
    }
}
