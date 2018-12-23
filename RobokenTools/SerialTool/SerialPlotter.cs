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
        private DataType _dataType = DataType.Byte;

        public SerialPlotter(Connection connection)
        {
            Connection = connection;
            Connection.SerialPort.DataReceived += SerialPort_DataReceived;
            UpdateThreashold(_dataType);
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            using (var br = new System.IO.BinaryReader(Connection.SerialPort.BaseStream,
                Connection.SerialPort.Encoding, true))
            {
                switch (DataType)
                {
                    case DataType.Byte:
                        Data.Add(br.ReadChar());
                        break;
                    case DataType.Int16:
                        Data.Add(br.ReadInt16());
                        break;
                    case DataType.Int32:
                        Data.Add(br.ReadInt32());
                        break;
                    case DataType.Int64:
                        Data.Add(br.ReadInt64());
                        break;
                    case DataType.UInt16:
                        Data.Add(br.ReadUInt16());
                        break;
                    case DataType.UInt32:
                        Data.Add(br.ReadUInt32());
                        break;
                    case DataType.UInt64:
                        Data.Add(br.ReadUInt64());
                        break;
                    case DataType.Float:
                        Data.Add(br.ReadSingle());
                        break;
                    case DataType.Double:
                        Data.Add(br.ReadDouble());
                        break;
                }
            }

            OnDataAvailable();
        }

        public Connection Connection { get; }

        public DataType DataType
        {
            get => _dataType;
            set
            {
                _dataType = value;

                Connection.SerialPort.DataReceived -= SerialPort_DataReceived;
                UpdateThreashold(value);
                Connection.SerialPort.DataReceived += SerialPort_DataReceived;
            }
        }

        private void UpdateThreashold(DataType value)
        {
            switch (value)
            {
                case DataType.Byte:
                    Connection.SerialPort.ReceivedBytesThreshold = 1;
                    break;
                case DataType.Int16:
                case DataType.UInt16:
                    Connection.SerialPort.ReceivedBytesThreshold = 2;
                    break;
                case DataType.UInt32:
                case DataType.Int32:
                case DataType.Float:
                    Connection.SerialPort.ReceivedBytesThreshold = 4;
                    break;
                case DataType.Int64:
                case DataType.UInt64:
                case DataType.Double:
                    Connection.SerialPort.ReceivedBytesThreshold = 8;
                    break;
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
