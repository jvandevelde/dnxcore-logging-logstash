using Microsoft.Extensions.Logging;

namespace JV.DotNetCore.Logging.Logstash
{
    /// <summary>
    /// The provider for the <see cref="EventLogLogger"/>.
    /// </summary>
    public class LogstashLoggerProvider : ILoggerProvider
    {
        private readonly LogstashLogSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLoggerProvider"/> class.
        /// </summary>
        public LogstashLoggerProvider()
            : this(settings: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLoggerProvider"/> class.
        /// </summary>
        /// <param name="settings">The <see cref="EventLogSettings"/>.</param>
        public LogstashLoggerProvider(LogstashLogSettings settings)
        {
            _settings = settings;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new LogstashLogger(name, _settings ?? new LogstashLogSettings());
        }

        public void Dispose()
        {
        }
    }
}
