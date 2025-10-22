// Systems/Events/EventPriority.cs
using System;

namespace Game.Systems.Events
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class)]
    public sealed class EventPriorityAttribute : Attribute
    {
        public int Priority { get; }
        public EventPriorityAttribute(int priority) => Priority = priority;
    }

    // Marcadores (use se não quiser usar atributo)
    public interface IGameplayEvent { }   // ex.: +100
    public interface IUIEvent { }         // ex.: +10
    public interface IAudioEvent { }      // ex.: 0
    public interface ITelemetryEvent { }  // ex.: -100

    public interface IEventPriorityProvider
    {
        int GetPriority(Type eventType);
    }

    public sealed class DefaultEventPriorityProvider : IEventPriorityProvider
    {
        public int GetPriority(Type eventType)
        {
            // 1) Atributo explícito vence
            var attr = (EventPriorityAttribute?)Attribute.GetCustomAttribute(eventType, typeof(EventPriorityAttribute));
            if (attr is not null) return attr.Priority;

            // 2) Marcadores
            if (typeof(IGameplayEvent).IsAssignableFrom(eventType)) return 100;
            if (typeof(IUIEvent).IsAssignableFrom(eventType)) return 10;
            if (typeof(IAudioEvent).IsAssignableFrom(eventType)) return 0;
            if (typeof(ITelemetryEvent).IsAssignableFrom(eventType)) return -100;

            // 3) Fallback
            return 0;
        }
    }
}
