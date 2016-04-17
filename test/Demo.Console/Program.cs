using JV.DotNetCore.Extensions.Logging.Logstash.Transports;

namespace Demo.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RedisTransport.Build("127.0.0.1", "6379", "logs")
                .Send("A formatted log message sent to Redis");
            UdpTransport.Build("127.0.0.1", 6379)
                .Send("A formatted log message set to Logstash directly over UDP");
        }
    }
}
