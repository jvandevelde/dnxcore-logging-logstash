using JV.DotNetCore.Extensions.Logging.Logstash.Transports;
using Microsoft.Extensions.Logging;
using System;

namespace JV.DotNetCore.Extensions.Logging.Logstash
{
    public class LogstashLogSettings
    {
        /// <summary>
        /// Name of the event log.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Defines where to send the formatted logs
        /// </summary>
        public ILogstashLogTransport LogTransport { get; set; }

        /// <summary>
        /// The function used to filter events based on the log level.
        /// </summary>
        public Func<string, LogLevel, bool> Filter { get; set; }
    }
}
