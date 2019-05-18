using System;
using System.Collections.Generic;

using RJCP.IO.Ports;

namespace serialport_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPortStream myPort = null;
            Console.WriteLine("Hello Serial!");
            Console.WriteLine(Environment.Version.ToString());
            string[] ports = GetPortNames();
            foreach (var port in ports)
                if (port == "/dev/ttyS0")
                {
                    myPort = new SerialPortStream("/dev/ttyS0", 115200, 8, Parity.None, StopBits.One);
                    myPort.Open();
                    if (!myPort.IsOpen)
                    {
                        Console.WriteLine("Error opening serial port");
                        return;
                    }
                    Console.WriteLine("Port open");
                }
            if (myPort == null)
            {
                Console.WriteLine("No serial port /dev/ttyS0");
                return;
            }
            myPort.Handshake = Handshake.None;
            myPort.ReadTimeout = 10000;
            myPort.NewLine = "\r\n";

            while (!Console.KeyAvailable)
            {
                try
                {
                    string readed = myPort.ReadLine();
                    Console.Write(readed);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

            }
        }
        public static string[] GetPortNames()
        {
            int p = (int)Environment.OSVersion.Platform;
            List<string> serial_ports = new List<string>();

            // Are we on Unix?
            if (p == 4 || p == 128 || p == 6)
            {
                string[] ttys = System.IO.Directory.GetFiles("/dev/", @"tty\*");
                foreach (string dev in ttys)
                {
                    if (dev.StartsWith("/dev/ttyS") || dev.StartsWith("/dev/ttyUSB") || dev.StartsWith("/dev/ttyACM") || dev.StartsWith("/dev/ttyAMA"))
                    {
                        serial_ports.Add(dev);
                        Console.WriteLine("Serial list: {0}", dev);
                    }
                }
            }
            return serial_ports.ToArray();
        }
    }
}