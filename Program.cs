using System;
using System.IO.Ports;
using Newtonsoft.Json;

namespace SerialPortExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the number of arguments.
            int argumentCount = args.Length;
            
            // Print the number of arguments.
            Console.WriteLine("The number of arguments is: {0}", argumentCount);

            Console.Write("Available Serial Ports:");

            string[] availablePorts = SerialPort.GetPortNames();
            List<Dictionary<string, string>> ports = new List<Dictionary<string, string>>();
            foreach (var portName in availablePorts)
            {
                using (SerialPort serialPort = new SerialPort(portName))
                {
                    try
                    {
                        serialPort.Open();
                        Dictionary<string, string> portData = new Dictionary<string, string>();
                        portData.Add("portName", portName);
                        portData.Add("baudrate", serialPort.BaudRate.ToString());
                        portData.Add("data", serialPort.DataBits.ToString());
                        portData.Add("parity", serialPort.Parity.ToString());
                        portData.Add("stopbit", serialPort.StopBits.ToString());
                        portData.Add("isopen", serialPort.IsOpen.ToString());
                        ports.Add(portData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accessing {portName}: {ex.Message}");
                    }
                }
            }

            // Get the output file name from the command line argument.
            // string fileName = args[0];
            // if (fileName == null)
            string fileName;
            if(argumentCount<1)
            {
                // Use the default output file name if no argument is provided.
                fileName = "serial_ports.json";
            }else{
                fileName=args[0];
            }

            // Create a JSON object from the list of ports.
            var json = JsonConvert.SerializeObject(ports);

            // Write the JSON object to the file.
            File.WriteAllText(fileName, json);

            Console.WriteLine($"JSON file created: {fileName}");
        }
    }
}
