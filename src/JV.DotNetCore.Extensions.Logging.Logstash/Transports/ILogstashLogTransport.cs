namespace JV.DotNetCore.Extensions.Logging.Logstash.Transports
{
    public interface ILogstashLogTransport
    {
        void Send(string message);
    }
}
