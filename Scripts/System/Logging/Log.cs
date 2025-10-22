// Systems/Logging/Log.cs
using System;
using System.Collections.Generic;
using Game.Systems.Events;

namespace Game.Systems.Logging
{
    public static class Log
    {
        // Filtro global por canal (habilitado/desabilitado)
        private static LogChannel _enabledChannels = LogChannel.All;

        // Min level por canal (padrão: Debug para tudo)
        private static readonly Dictionary<LogChannel, LogLevel> _minLevel = new();

        static Log()
        {
            foreach (LogChannel ch in Enum.GetValues(typeof(LogChannel)))
            {
                if (ch == LogChannel.None || ch == LogChannel.All) continue;
                _minLevel[ch] = LogLevel.Debug;
            }
        }

        // ---- Config API (global) ----
        public static void Enable(LogChannel ch) => _enabledChannels |= ch;
        public static void Disable(LogChannel ch) => _enabledChannels &= ~ch;
        public static void SetMinLevel(LogChannel ch, LogLevel level) => _minLevel[ch] = level;

        public static bool IsEnabled(LogChannel ch, LogLevel level)
        {
            if ((_enabledChannels & ch) == 0) return false;
            if (!_minLevel.TryGetValue(ch, out var min)) min = LogLevel.Debug;
            return level >= min;
        }

        // ---- Emissão ----
        public static void Write(LogChannel ch, LogLevel level, string message, string? sourceId = null)
        {
            if (!IsEnabled(ch, level)) return;
            GlobalBus.Publish(new LogMessage(ch, level, message, sourceId));
        }

        // Facades
        public static void Trace(LogChannel ch, string msg, string? src = null) => Write(ch, LogLevel.Trace, msg, src);
        public static void Debug(LogChannel ch, string msg, string? src = null) => Write(ch, LogLevel.Debug, msg, src);
        public static void Info(LogChannel ch, string msg, string? src = null) => Write(ch, LogLevel.Info, msg, src);
        public static void Warn(LogChannel ch, string msg, string? src = null) => Write(ch, LogLevel.Warn, msg, src);
        public static void Error(LogChannel ch, string msg, string? src = null) => Write(ch, LogLevel.Error, msg, src);
    }
}
