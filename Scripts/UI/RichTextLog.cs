// UI/RichTextLog.cs
using Godot;
using Game.Systems.Events;
using Game.Systems.Logging;

public partial class RichTextLog : RichTextLabel
{
    [Export] public LogChannel Channels = LogChannel.All;
    [Export] public LogLevel MinLevel = LogLevel.Debug;
    [Export] public bool ShowLevel = true;
    [Export] public bool AutoScroll = true;

    public override void _Ready()
    {
        BbcodeEnabled = true;
        GlobalBus.Subscribe<LogMessage>(this, OnLog);
    }

    private void OnLog(LogMessage e)
    {
        if ((Channels & e.Channel) == 0) return;
        if (e.Level < MinLevel) return;

        var tag = LogStyle.TagFor(e.Channel);
        var color = LogStyle.ColorFor(e.Channel);
        var lvl = ShowLevel ? $" {e.Level}" : "";
        AppendText($"[color={color}][{tag}]{lvl}[/color] {e.Message}\n");

        if (AutoScroll) ScrollToLine(GetLineCount() - 1);
    }
}