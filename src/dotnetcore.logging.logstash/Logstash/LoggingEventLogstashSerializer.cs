using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace logstash.logging.Logging.Logstash
{
    /// <summary>
    /// Takes data from the DNX logging infrastructure and converts it into a JSON document
    /// with a schema that Logstash understands (v1 schema).
    /// Basically, there are only two required fields 
    ///    @version (string) - newest should always be set to "1"
    ///    @timestamp (string) - a ISO8601 high-precision timestamp
    /// All other valid JSON fields will be treated as normal fields by Logstash
    /// See https://gist.github.com/jordansissel/2996677
    /// </summary>
    public class LoggingEventLogstashSerializer
    {
        public const string ISO8601DatetimeTimeZoneFormatWithMillis = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";
        private const int LogstashJsonEventVersion = 1;

        private IDictionary<string, string> _exceptionInformation;
        private JObject _logstashEvent;

        private void AddEventData(string keyname, string keyval)
        {
            if (keyval != null)
            {
                _logstashEvent.Add(keyname, new JValue(keyval));
            }
            else
            {
                _logstashEvent.Add(keyname, string.Empty);
            }
        }

        public string GetFormattedMessage(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            _exceptionInformation = new SortedDictionary<string, string>();

            _logstashEvent = new JObject();

            _logstashEvent.Add("@version", new JValue(LogstashJsonEventVersion));
            AddEventData("@timestamp", DateTime.Now.ToString(ISO8601DatetimeTimeZoneFormatWithMillis));
            AddEventData("source_host", "gethost-xplat");
            AddEventData("level", logLevel.ToString());
            AddEventData("event_id", eventId.ToString());
            AddEventData("message", state.ToString());

            BuildExceptionInformation(exception);

            return JsonConvert.SerializeObject(_logstashEvent, Formatting.None);
        }

        private void BuildExceptionInformation(Exception ex)
        {
            if (ex != null)
            {
                AddEventData("exception_class", ex.GetType().Name);
                AddEventData("exception_message", ex.Message);
                AddEventData("stacktrace", ex.StackTrace);
            }
        }
    }
}
