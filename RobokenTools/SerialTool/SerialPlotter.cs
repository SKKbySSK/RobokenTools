using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RobokenTools.SerialTool
{
    public enum DataType
    {
        Byte,
        Int64,
        UInt64,
        Int32,
        UInt32,
        Int16,
        UInt16,
        Float,
        Double
    }

    class SerialPlotter : Abstracts.DataPlotter
    {
        private DataType _dataType = DataType.Float;

        public SerialPlotter(Connection connection)
        {
            Connection = connection;
            Connection.SerialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            lock (Buffer)
            {
                using (var br = new System.IO.BinaryReader(Connection.SerialPort.BaseStream,
                    Connection.SerialPort.Encoding, true))
                {
                    switch (DataType)
                    {
                        case DataType.Float:
                            Data.Add(br.ReadSingle());
                            break;
                    }
                }

                OnDataAvailable();
            }
        }

        private System.IO.MemoryStream Buffer { get; } = new System.IO.MemoryStream();
        
        public TimeSpan Interval { get; } = TimeSpan.FromMilliseconds(100);

        public Connection Connection { get; }

        public DataType DataType
        {
            get => _dataType;
            set
            {
                _dataType = value;

                Connection.SerialPort.DataReceived -= SerialPort_DataReceived;
                switch (value)
                {
                    case DataType.UInt32:
                    case DataType.Int32:
                    case DataType.Float:
                        Connection.SerialPort.ReceivedBytesThreshold = 4;
                        break;
                }
                Connection.SerialPort.DataReceived += SerialPort_DataReceived;
            }
        }

        public override void Close()
        {
            Connection.Disconnect();
        }

        public override void Open()
        {
            Connection.Connect();
        }
    }
}
