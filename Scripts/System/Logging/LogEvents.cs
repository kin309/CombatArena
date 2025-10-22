using Game.Systems.Events;
using System;

namespace Game.Systems.Logging
{
    public enum LogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
    }

    [Flags]
    public enum LogChannel
    {
        None = 0,
        Combat = 1 << 0,
        UI = 1 << 1,
        AI = 1 << 2,
        System = 1 << 3,
        Net = 1 << 4,
        Audio = 1 << 5,
        All = ~0
    }

    public readonly record struct LogMessage(
        LogChannel Channel,
        LogLevel Level,
        string Message,
        string? SourceId = null
    ) : ITelemetryEvent;
}