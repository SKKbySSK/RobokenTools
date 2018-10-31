using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobokenTools.SerialTool
{
    class SerialPlotter : Abstracts.DataPlotter
    {
        public SerialPlotter(Connection connection)
        {
            Connection = connection;
        }

        public Connection Connection { get; }

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
