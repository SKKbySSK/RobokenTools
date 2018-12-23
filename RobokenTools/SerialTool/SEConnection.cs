namespace RobokenTools.SerialTool
{
    class SEConnection
    {
        public SEConnection(Connection connection, byte start, byte end)
        {
            Connection = connection;
            Start = start;
            End = end;
        }

        public Connection Connection { get; }

        public byte Start { get; }

        public byte End { get; }
    }
}
