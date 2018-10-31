using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Diagnostics;

namespace RobokenTools.SerialTool
{
    public static class ConnectionManager
    {
        public static List<Connection> GetPorts()
        {
            var names = SerialPort.GetPortNames();
            List<Connection> connections = new List<Connection>();

            foreach (var n in names)
            {
                try
                {
                    connections.Add(new Connection(n));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            return connections;
        }
    }
}