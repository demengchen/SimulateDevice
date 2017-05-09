using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueMsg
{
    /// <summary>
    /// Test queue message to Azure Service Bus queue "test".
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Service Bus connection string
                var connectionString = "Endpoint=sb://eatoniotservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tvFviHvCrxOYWOSU5FgoJpfUz/UOqOFAVdFUmTUkCWg=";
                // Service Bus queue name
                var queueName = "test";

                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);
                var message = new BrokeredMessage("This is a test message!");
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
