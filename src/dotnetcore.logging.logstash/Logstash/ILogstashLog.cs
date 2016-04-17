namespace JV.DotNetCore.Logging.Logstash
{ 
    public interface ILogstashLog
    {
        int MaxMessageSize { get; }

        void WriteEntry(string message, int eventID, short category);
    }
}
