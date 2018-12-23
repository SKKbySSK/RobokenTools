using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobokenTools.SerialTool
{
    class SESerialPlotter : Abstracts.DataPlotter
    {
        SerialPlotter SerialPlotter;
        SEConnection Connection;

        public SESerialPlotter(SEConnection connection)
        {
            Connection = connection;
            SerialPlotter = new SerialPlotter(connection.Connection);
            SerialPlotter.DataAvailable += SerialPlotter_DataAvailable;
        }

        private void SerialPlotter_DataAvailable(object sender, EventArgs e)
        {
            var last = SerialPlotter.Data[SerialPlotter.Data.Count - 1];
            if (SerialPlotter.DataType == DataType.Byte)
            {
                var b = (byte)last.Value;

                if (b == Connection.Start)
                    SerialPlotter.DataType = DataType.Float;
            }
            else if (SerialPlotter.DataType == DataType.Float)
            {
                Data.Add(last.Value);
                OnDataAvailable();
                SerialPlotter.DataType = DataType.Byte;
            }
        }

        public override void Close()
        {
            SerialPlotter.Close();
        }

        public override void Open()
        {
            SerialPlotter.Open();
        }
    }
}
