using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JV.DotNetCore.Extensions.Logging.Logstash.Transports
{
    public class UdpTransport : ILogstashLogTransport
    {
        private static UdpClient _client { get; set; }
        private string _host;
        private int _port;

        public UdpTransport(string host, int port)
        {
            _host = host;
            _port = port;
            _client = new UdpClient();
        }

        public static UdpTransport Build(string host, int port)
        {
            return new UdpTransport(host, port);
        }

        public void Send(string log)
        {
            var _endpoint = new IPEndPoint(IPAddress.Parse(_host), _port);

            var bytes = Encoding.ASCII.GetBytes(log);
            _client.SendAsync(bytes, bytes.Length, _endpoint);
        }
    }
}
