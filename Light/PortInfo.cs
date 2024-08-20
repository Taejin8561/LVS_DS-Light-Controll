namespace Light
{
    public class PortInfo
    {
        public string Port { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public string StopBits { get; set; }
        public string Parity { get; set; }
        public string HandShake { get; set; }
    }
}