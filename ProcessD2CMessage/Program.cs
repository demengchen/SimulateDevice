using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Threading;

namespace ProcessD2CMessage
{
    class Program
    {
        static string connectionString = "HostName=eatoniothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=+5XRVnVw+3cy1AeZugU4r5iCu6j4VVe6OgVVWpz+ukM=";
            //"HostName=EatonDevIothub2.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=zeGWxiap/mPuFyew3lIH8ZV+O3MLfUU1CXGp124Lr/U=";
        //                               "HostName=EatonDevIothub2.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=zeGWxiap/mPuFyew3lIH8ZV+O3MLfUU1CXGp124Lr/U=";
        static string iotHubD2cEndpoint = "messages/events";
        static EventHubClient eventHubClient;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Receive messages. Ctrl-C to exit.\n");
                eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, iotHubD2cEndpoint);

                var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

                CancellationTokenSource cts = new CancellationTokenSource();

                System.Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                    Console.WriteLine("Exiting...");
                };

                var tasks = new List<Task>();
                foreach (string partition in d2cPartitions)
                {
                    tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
                }
                Task.WaitAll(tasks.ToArray());
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            DateTime dt = DateTime.UtcNow - TimeSpan.FromDays(1);
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, dt);
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
            }
        }
    }
}
