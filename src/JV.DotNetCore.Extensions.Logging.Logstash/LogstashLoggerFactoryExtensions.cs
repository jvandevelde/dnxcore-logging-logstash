using Microsoft.Extensions.Logging;
using System;

namespace JV.DotNetCore.Extensions.Logging.Logstash
{
    public static class LogstashLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds an event logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        public static ILoggerFactory AddLogstashLog(this ILoggerFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return AddLogstashLog(factory, LogLevel.Information);
        }

        /// <summary>
        /// Adds an event logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        public static ILoggerFactory AddLogstashLog(this ILoggerFactory factory, LogLevel minLevel)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return AddLogstashLog(factory, new LogstashLogSettings()
            {
                Filter = (_, logLevel) => logLevel >= minLevel
            });
        }

        /// <summary>
        /// Adds an event logger. Use <paramref name="settings"/> to enable logging for specific <see cref="LogLevel"/>s.
        /// </summary>
        /// <param name="factory">The extension method argument.</param>
        /// <param name="settings">The <see cref="EventLogSettings"/>.</param>
        public static ILoggerFactory AddLogstashLog(
            this ILoggerFactory factory,
            LogstashLogSettings settings)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            factory.AddProvider(new LogstashLoggerProvider(settings));
            return factory;
        }
    }
}
