using Microsoft.Extensions.Logging;
using System;

namespace JV.DotNetCore.Logging.Logstash
{
    public class LogstashLogSettings
    {
        /// <summary>
        /// Name of the event log. If <c>null</c> or not specified, "Application" is the default.
        /// </summary>
        public string LogName { get; set; }

        /// <summary>
        /// Name of the event log source. If <c>null</c> or not specified, "Application" is the default.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Name of the machine having the event log. If <c>null</c> or not specified, local machine is the default.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// The function used to filter events based on the log level.
        /// </summary>
        public Func<string, LogLevel, bool> Filter { get; set; }

        /// <summary>
        /// For unit testing purposes only.
        /// </summary>
        public ILogstashLog EventLog { get; set; }
    }
}
