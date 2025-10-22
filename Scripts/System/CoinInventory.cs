// Systems/CoinInventory.cs
using Godot;
using Game.Systems.Events;
using Game.Systems.Logging;

public partial class CoinInventory : Node
{
    private int _coins;

    public override void _Ready()
    {
        // Auto-dispose porque passamos 'this'
        GlobalBus.Instance.Subscribe<CoinCollected>(this, e =>
        {
            _coins += e.Amount; // 1) aplica regra de jogo
            // 2) notifica UI com o novo total
            GlobalBus.Instance.Publish(new CoinTotalChanged(_coins));
            Log.Info(LogChannel.Combat, $"Moeda coletada +{e.Amount}");
        });

    }
}
