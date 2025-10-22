// Systems/Events/GlobalBus.cs
using System;

namespace Game.Systems.Events
{
    public static class GlobalBus
    {
        // Instância única, inicializada uma vez
        public static readonly IEventBus Instance = new OwnerAwareEventBus();

        // Opcional: helper rápido para publish/subscribe
        public static void Publish<TEvent>(TEvent ev) => Instance.Publish(ev);
        public static IDisposable Subscribe<TEvent>(Godot.Node owner, Action<TEvent> handler, int offset = 0)
            => Instance.Subscribe(owner, handler, offset);
    }
}
