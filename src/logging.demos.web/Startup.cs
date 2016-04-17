using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JV.DotNetCore.Logging.Logstash;

namespace logging.demos.web
{
    public class Startup
    {
        private static int _eventId = 0;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("Catchall Endpoint");
            loggerFactory.AddConsole();
            loggerFactory.AddLogstashLog();
            logger.Log(LogLevel.Critical, ++_eventId, "Something has gone horribly wrong", new Exception("Weirdness Occurred"), null);

            app.Run(async (context) =>
            {
                logger.Log(LogLevel.Verbose, _eventId, string.Format("Incoming request to: {0}", context.Request.Path), null, null);
                await context.Response.WriteAsync("Hello World!");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
