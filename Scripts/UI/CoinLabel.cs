// UI/CoinLabel.cs
using Godot;
using Game.Systems.Events;

public partial class CoinLabel : Label
{
    private int _coins;

    public override void _Ready()
    {
        GlobalBus.Instance.Subscribe<CoinTotalChanged>(this, e =>
        {
            _coins = e.NewTotal;
            Text = $"Coins: {_coins}";
        });
    }
}
