using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace JV.DotNetCore.Extensions.Logging.Logstash.Transports
{
    /// <summary>
    /// A transport to send data to Redis
    /// 
    /// NOTE: StackExchange.Redis package is prerelease due to requiring changes in DNX RC2
    /// </summary>
    public class RedisTransport : ILogstashLogTransport
    {
        private static ConnectionMultiplexer _redisMultiplexer;
        private string _redisListKey;

        public RedisTransport(string host, string port, string redisListKey)
        {
            _redisListKey = redisListKey;

            var redisConnectionString =
                string.Format("{0}:{1}", host, port);
            
            _redisMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public static RedisTransport Build(string host, string port, string redisListKey)
        {
            return new RedisTransport(host, port, redisListKey);
        }

        public void Send(string log)
        {
            IDatabase _redisDb = _redisMultiplexer.GetDatabase();
            _redisDb.ListRightPush(_redisListKey, log);
        }
    }
}
