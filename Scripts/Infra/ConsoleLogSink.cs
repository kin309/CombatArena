// Infra/ConsoleLogSink.cs
using Godot;
using Game.Systems.Events;
using Game.Systems.Logging;

public partial class ConsoleLogSink : Node
{
    // Filtro local (por sink): exiba apenas estes canais
    [Export] public LogChannel Channels = LogChannel.All;
    [Export] public LogLevel MinLevel = LogLevel.Debug;

    public override void _Ready()
    {
        GlobalBus.Subscribe<LogMessage>(this, OnLog);
    }

    private void OnLog(LogMessage e)
    {
        if ((Channels & e.Channel) == 0) return;
        if (e.Level < MinLevel) return;

        var tag = LogStyle.TagFor(e.Channel);
        GD.Print($"[{tag}] {e.Level}: {e.Message}");
    }
}
