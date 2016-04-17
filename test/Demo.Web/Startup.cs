using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using JV.DotNetCore.Extensions.Logging.Logstash;
using JV.DotNetCore.Extensions.Logging.Logstash.Transports;

namespace Demo.Web
{
    public class Startup
    {
        private static int _eventId = 0;
        private static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            LoadConfiguration();
            ConfigureLogging(loggerFactory);

            var logger = loggerFactory.CreateLogger("Catchall Endpoint");
            logger.Log(LogLevel.Critical, ++_eventId, "Something has gone horribly wrong", new Exception("Weirdness Occurred"), null);

            app.Run(async (context) =>
            {
                logger.Log(LogLevel.Verbose, _eventId, string.Format("Incoming request to: {0}", context.Request.Path), null, null);
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private static void LoadConfiguration()
        {
            // dotnetcore configuration https://msdn.microsoft.com/en-us/magazine/mt632279.aspx
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("config.json");

            Configuration = configurationBuilder.Build();
        }

        private static void ConfigureLogging(ILoggerFactory loggerFactory)
        {
            var redisHost = Configuration["AppConfiguration:Redis:Server"];
            var redisPort = Configuration["AppConfiguration:Redis:Port"];
            var redisListKey = Configuration["AppConfiguration:Redis:ListKey"];

            loggerFactory.AddConsole();
            loggerFactory.AddLogstashLog(new LogstashLogSettings()
            {
                Filter = (_, logLevel) => logLevel >= LogLevel.Verbose,
                LogTransport = new RedisTransport(redisHost, redisPort, redisListKey),
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
