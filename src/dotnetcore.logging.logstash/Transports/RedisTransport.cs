using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace JV.DotNetCore.Logging.Logstash.Transports
{
    /// <summary>
    /// A transport to send data to Redis
    /// 
    /// NOTE: StackExchange.Redis package is prerelease due to requiring changes in DNX RC2
    /// </summary>
    public class RedisTransport
    {
        private static ConnectionMultiplexer _redisMultiplexer;
        
        static public IConfiguration Configuration { get; set; }

        static RedisTransport()
        {
            // dotnetcore configuration https://msdn.microsoft.com/en-us/magazine/mt632279.aspx
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("config.json");

            Configuration = configurationBuilder.Build();

            var redisConnectionString = 
                string.Format("{0}:{1}", Configuration["AppConfiguration:Redis:Server"],
                        Configuration["AppConfiguration:Redis:Port"]);
            _redisMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public static void Send(string log)
        {
            IDatabase _redisDb = _redisMultiplexer.GetDatabase();
            _redisDb.ListRightPush(Configuration["AppConfiguration:Redis:ListKey"], log);
        }
    }
}
