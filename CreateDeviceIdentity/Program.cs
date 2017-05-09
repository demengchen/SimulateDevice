using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Configuration;
using CmdLine;

namespace CreateDeviceIdentity
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CommandLine.CaseSensitive = false;
                CreateDeviceArguments arguments = CommandLine.Parse<CreateDeviceArguments>();

                if (String.IsNullOrEmpty(arguments.IoTHubConnectionString))
                {
                    arguments.IoTHubConnectionString = GetConfigString("iot_hub_cnn");
                }

                if (String.IsNullOrEmpty(arguments.DeviceId))
                {
                    arguments.DeviceId = GetConfigString("dev_id");
                }

                if (!String.IsNullOrEmpty(arguments.IoTHubConnectionString))
                    AddDeviceAsync(arguments.IoTHubConnectionString, arguments.DeviceId).Wait();
                else
                    Console.WriteLine("There is no IoT Hub connection string.");

            } catch (CommandLineException ex)
            {
                Console.WriteLine(ex.ArgumentHelp.Message);
                Console.WriteLine(ex.ArgumentHelp.GetHelpText(Console.BufferWidth));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static String GetConfigString(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings[key];
        }

        private static async Task AddDeviceAsync(string connectionString, string deviceId)
        {
            RegistryManager registryManager = null;
            Device device = null;

            if (String.IsNullOrEmpty(deviceId))
                deviceId = Guid.NewGuid().ToString();

            try
            {
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
                Console.WriteLine("Device was created successfully.");
            }
            catch (DeviceAlreadyExistsException)
            {
                Console.WriteLine("Device {0} already exists", deviceId);
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Created info :- \n\tid: {0}; \n\tkey: {1}", device.Id, device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
