namespace Game.Systems.Events
{
    public interface IGameEvent
    {
        string Type { get; }
        string SourceId { get; }
        object? Payload { get; }
    }

    public readonly record struct GameEvent(string Type, string SourceId, object? Payload = null) : IGameEvent;

    public readonly record struct CoinCollected(int Amount) : IGameplayEvent;
    public readonly record struct CoinTotalChanged(int NewTotal) : IUIEvent;

    public static class EventTypes
    {
        public const string CoinCollected = "Coin.Collected";
    }
}