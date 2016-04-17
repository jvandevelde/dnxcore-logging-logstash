using System;
using System.Threading;

namespace JV.DotNetCore.Extensions.Logging.Logstash
{
    public class LogstashLogScope
    {
        private readonly string _name;
        private readonly object _state;

        internal LogstashLogScope(string name, object state)
        {
            _name = name;
            _state = state;
        }

        public LogstashLogScope Parent { get; private set; }

        private static AsyncLocal<LogstashLogScope> _value = new AsyncLocal<LogstashLogScope>();
        public static LogstashLogScope Current
        {
            set
            {
                _value.Value = value;
            }
            get
            {
                return _value.Value;
            }
        }

        public static IDisposable Push(string name, object state)
        {
            var temp = Current;
            Current = new LogstashLogScope(name, state);
            Current.Parent = temp;

            return new DisposableScope();
        }

        public override string ToString()
        {
            return _state?.ToString();
        }

        private class DisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
