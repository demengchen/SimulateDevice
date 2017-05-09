using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string deviceId = "50000000-6666-7777-8888-999999999999"; //SharedAccessKey=+4O4dcVqhZEkk+BCWZFmED0o4JFZIdsSbUKembLQFSU=";
        static string deviceKey = "vyQ2EXDBklTnpmBLj03rWjHK2+H0LHT5bmDt9n87rXs="; //" + 4O4dcVqhZEkk+BCWZFmED0o4JFZIdsSbUKembLQFSU=";
        static string iotHubUri = "eatoniothub.azure-devices.net";
        

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(
                iotHubUri, 
                new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), 
                TransportType.Mqtt_WebSocket_Only);

            try
            {
                SendDeviceToCloudMessagesAsync();
            } catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
            Console.ReadLine();
        }

        //private static async void SendDeviceToCloudMessagesAsync()
        //{
        //    double minTemperature = 20;
        //    double minHumidity = 60;
        //    int count = 0;
        //    Random rand = new Random();

        //    while (count < 20)
        //    {
        //        double currentTemperature = minTemperature + rand.NextDouble() * 15;
        //        double currentHumidity = minHumidity + rand.NextDouble() * 20;

        //        var telemetryDataPoint = new
        //        {
        //            deviceId = "myFirstDevice",
        //            temperature = currentTemperature,
        //            humidity = currentHumidity
        //        };
        //        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
        //        var message = new Message(Encoding.ASCII.GetBytes(messageString));
        //        message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

        //        await deviceClient.SendEventAsync(message);
        //        Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

        //        await Task.Delay(1000);
        //        count++;
        //    }
        //}

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();
            int count = 0;

            while (count < 20)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telemetryDataPoint = new
                {
                    deviceId = "myFirstDevice",
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                string levelValue;

                if (rand.NextDouble() > 0.7)
                {
                    messageString = "This is a critical message";
                    levelValue = "critical";
                }
                else
                {
                    levelValue = "normal";
                }

                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("level", levelValue);

                await deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sent message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
                count++;
            }
        }
    }
}
