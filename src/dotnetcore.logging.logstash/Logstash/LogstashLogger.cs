using JV.DotNetCore.Logging.Logstash.Transports;
using Microsoft.Extensions.Logging;
using System;

namespace JV.DotNetCore.Logging.Logstash
{
    public class LogstashLogger : ILogger
    {
        private readonly string _name;
        private readonly LogstashLogSettings _settings;

        public LogstashLogger(string name, LogstashLogSettings settings)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(LogstashLogger) : name;
            _settings = settings;

            var logName = string.IsNullOrEmpty(settings.LogName) ? "Application" : settings.LogName;
            var sourceName = string.IsNullOrEmpty(settings.SourceName) ? "Application" : settings.SourceName;
            var machineName = string.IsNullOrEmpty(settings.MachineName) ? "." : settings.MachineName;

        }

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            var x = new LoggingEventLogstashSerializer();
            var logData = x.GetFormattedMessage(logLevel, eventId, state, exception, formatter);
            RedisTransport.Send(logData);
        }
    }
}
