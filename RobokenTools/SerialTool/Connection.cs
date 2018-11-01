using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Newtonsoft.Json;

namespace RobokenTools.SerialTool
{
    public class Connection : IDisposable
    {
        public Connection(string portName)
        {
            PortName = portName;
            SerialPort = new SerialPort(portName);
        }

        [JsonConstructor]
        public Connection(string portName, int baudRate, int dataBits, Parity parity) : this(portName)
        {
            BaudRate = baudRate;
            DataBits = dataBits;
            Parity = parity;
        }

        public SerialPort SerialPort { get; }

        public int Read(byte[] buffer, int offset, int count)
        {
            return SerialPort.Read(buffer, offset, count);
        }

        public byte[] Read(int count)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; count > i; i++)
            {
                buffer[i] = (byte)SerialPort.ReadByte();
            }
            return buffer;
        }

        public string PortName { get; }

        public int BaudRate
        {
            get => SerialPort.BaudRate;
            set => SerialPort.BaudRate = value;
        }

        public int DataBits
        {
            get => SerialPort.DataBits;
            set => SerialPort.DataBits = value;
        }

        public Parity Parity
        {
            get => SerialPort.Parity;
            set => SerialPort.Parity = value;
        }

        public bool IsConnected { get; private set; } = false;

        public void Connect()
        {
            SerialPort.Open();
            IsConnected = SerialPort.IsOpen;
        }

        public void Disconnect()
        {
            SerialPort.Close();
            IsConnected = SerialPort.IsOpen;
        }

        public void Dispose()
        {
            Disconnect();
            SerialPort.Dispose();
        }
    }
}