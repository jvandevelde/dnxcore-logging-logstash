using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace JV.DotNetCore.Extensions.Logging.Logstash
{
    public class LogstashLogger : ILogger
    {
        private readonly string _name;
        private readonly LogstashLogSettings _settings;
        private readonly LoggingEventLogstashSerializer _serializer;

        public LogstashLogger(string name, LogstashLogSettings settings)
        {
            _name = string.IsNullOrEmpty(name) ? nameof(LogstashLogger) : name;
            _settings = settings;
            _serializer = new LoggingEventLogstashSerializer();
        }

        public IDisposable BeginScopeImpl(object state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return LogstashLogScope.Push(_name, state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            var logData = _serializer.GetFormattedMessage(logLevel, eventId, state, GetScopeInformation(), exception, formatter);
            _settings.LogTransport.Send(logData);
        }

        private string GetScopeInformation()
        {
            var current = LogstashLogScope.Current;
            var output = new StringBuilder();
            string scopeLog = string.Empty;
            while (current != null)
            {
                if (output.Length == 0)
                {
                    scopeLog = $"=> {current}";
                }
                else
                {
                    scopeLog = $"=> {current} ";
                }

                output.Insert(0, scopeLog);
                current = current.Parent;
            }

            return output.ToString();
        }

    }
}
