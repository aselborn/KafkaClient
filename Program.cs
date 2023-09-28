
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
            Console.WriteLine("Felaktigt argument");
            return;
        }
        if (args.Length != 0)
        {
            if (args[0].CompareTo("-r") == 0)
            {
                Console.WriteLine($"Lyssnar efter inkommande meddelanden.");
                await Task.Factory.StartNew(() =>
                {
                    new KafkaMessageConsumer().Run(new CancellationToken()!);
                });
                Console.ReadKey();
            }

            if (args[0].CompareTo("-m") == 0)
            {
                Console.WriteLine($"Program start, avluta med A: ");

                await Task.Factory.StartNew(() =>
                {
                    new KafkaMessageConsumer().Run(new CancellationToken()!);
                });

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

            if (args[0].CompareTo("-n") == 0) //massmeddelanden...
            {
                Console.Write("Antal meddelanden : ");
                var reps = Console.ReadLine()!;
                if (int.TryParse(reps, out var result))
                {
                    for (int n = 0; n < result; n++)
                    {
                        var msg = new WorkQueue.topic.v1.WorkQueue()
                        {
                            cdhTerminalCode = n.ToString(),
                            eventType = string.Concat("event_type_", n.ToString()),
                            load_mode = string.Concat("load_moad_", n.ToString()),
                            messageSequenceNumber = 10000 + n,
                            opType = string.Concat("op_type_", n.ToString()),
                            pointOfWorkName = string.Concat("point_of_workname_", n.ToString()),
                            SOURCE_TS_MS = 10001 + n
                        };

                        new KafkaMessageClient().SendMessage(msg);
                    }


                }

            }

            Console.WriteLine($"Argument stöds inte");
            return;

        }
       
        
    }

}