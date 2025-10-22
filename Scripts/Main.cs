// Nodes/Game.cs
using Godot;
using Game.Systems.Events;

public partial class Main : Node
{

    public override void _Ready()
    {
        // mapear tecla rapidamente (se já tiver, pode remover)
        if (!InputMap.HasAction("ui_accept"))
        {
            InputMap.AddAction("ui_accept");
            InputMap.ActionAddEvent("ui_accept", new InputEventKey { Keycode = Key.Space });
            InputMap.ActionAddEvent("ui_accept", new InputEventKey { Keycode = Key.Enter });
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            GlobalBus.Instance.Publish(new CoinCollected(1));
        }
    }
}
