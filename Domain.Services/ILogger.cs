namespace EtGate.Domain.Services
{
    public enum LogLevel
    {
        Debug,
        Verbose,
        Info,
        Warn,
        Error,
        Fatal,
    };
    public interface ILogger
    {
        void LogDebug(string message);
        void LogDebug(Func<string> message);

        void LogVerbose(string message);
        void LogVerbose(Func<string> message);

        void LogInfo(string message);
        void LogInfo(Func<string> message);

        void LogWarning(string message);
        void LogWarning(Func<string> message);

        void LogError(string message);
        void LogError(Func<string> message);

        void LogFatal(string message);
        void LogFatal(Func<string> message);

        //LogLevel LogLevel { get; }
    }
}
