using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JV.DotNetCore.Logging.Logstash.Transports
{
    public class UdpTransport
    {
        static public IConfiguration Configuration { get; set; }
        static private UdpClient _client { get; set; }

        static UdpTransport()
        {
            // dotnetcore configuration https://msdn.microsoft.com/en-us/magazine/mt632279.aspx
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("config.json");

            Configuration = configurationBuilder.Build();
            
            _client = new UdpClient();
        }

        public static void Send(string log)
        {
            var serverIp = Configuration["AppConfiguration:LogstashUdp:Server"];
            var serverPort = Convert.ToInt32(Configuration["AppConfiguration:LogstashUdp:Port"]);

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

            var bytes = Encoding.ASCII.GetBytes(log);
            _client.SendAsync(bytes, bytes.Length, ep);
        }
    }
}
