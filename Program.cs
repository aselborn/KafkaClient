
using Confluent.Kafka;
using static Confluent.Kafka.ConfigPropertyNames;
using WorkQueue.topic.v1;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using kafkaclient;

internal class Program
{
    private static async Task Main(string[] args)
    {
        
        if (args.Length == 0)
        {
            if (args[0].CompareTo("-m") != 0)
            {
                Console.WriteLine($"Ange argumen till programmet -m för meddelande");
                return;
            }

        }
        else
        {
            Console.WriteLine($"Program start, avluta med A");
            
            

            await Task.Factory.StartNew(() =>
            {
                new KafkaMessageConsumer().Run(new CancellationToken()!);
            });

            
        }

        while (true)
        {
            Console.Write("Ange fartygsnamn ");
            var input = Console.ReadLine()!;
            if (input.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("A avslutade programmet.");
                return;
            }


            var theMessage = new WorkQueue.topic.v1.WorkQueue()
            {
                cdhTerminalCode = "1",
                eventType = "ev type",
                pointOfWorkName = "point of workname",
                vesselName = input,
                vesselRow = "0"
            };

            new KafkaMessageClient().SendMessage(theMessage);
            Console.WriteLine();

        }


     
        
    }

}