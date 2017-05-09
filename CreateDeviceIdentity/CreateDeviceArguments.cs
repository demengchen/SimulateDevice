using CmdLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateDeviceIdentity
{
    /// <summary>
    /// 
    /// </summary>
    [CommandLineArguments(Program = "CreateDeviceIdentity", Title = "Create and register device in IoT Hub", Description = "Create and register device in IoT Hub command")]
    public class CreateDeviceArguments
    {
        [CommandLineParameter(Command = "?", Default = false, Description = "Show Help", Name = "Help", IsHelp = true)]
        public bool Help { get; set; }

        [CommandLineParameter(Command = "c", ParameterIndex = 1, Required = false, Description = "IoT Hub connection string. If not specified, use the 'iot_hub_cnn' in config file.")]
        public string IoTHubConnectionString { get; set; }

        [CommandLineParameter(Command = "d", ParameterIndex = 2, Required = false, Description = "Device id. If not specified, use 'dev_id' in config file if specified. Otherwise, use generated guid as device id.")]
        public string DeviceId { get; set; }
    }
}
