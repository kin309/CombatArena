// Systems/Logging/LogStyle.cs
using System.Collections.Generic;

namespace Game.Systems.Logging
{
    public static class LogStyle
    {
        // Tag por canal (aparece como [TAG])
        public static readonly Dictionary<LogChannel, string> Tag = new()
        {
            [LogChannel.Combat] = "COMBAT",
            [LogChannel.UI] = "UI",
            [LogChannel.AI] = "AI",
            [LogChannel.System] = "SYS",
            [LogChannel.Net] = "NET",
            [LogChannel.Audio] = "SFX",
        };

        // Cores BBCode (Godot RichTextLabel)
        public static readonly Dictionary<LogChannel, string> Color = new()
        {
            [LogChannel.Combat] = "#ff6b6b",
            [LogChannel.UI] = "#4dabf7",
            [LogChannel.AI] = "#ffd43b",
            [LogChannel.System] = "#94d82d",
            [LogChannel.Net] = "#22b8cf",
            [LogChannel.Audio] = "#b197fc",
        };

        public static string TagFor(LogChannel ch) => Tag.TryGetValue(ch, out var t) ? t : ch.ToString().ToUpper();
        public static string ColorFor(LogChannel ch) => Color.TryGetValue(ch, out var c) ? c : "#cccccc";
    }
}
