using System;
using Godot;

namespace Game.Systems.Events
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent ev);

        // Assina com "owner": auto-dispose quando o Node sair da árvore
        IDisposable Subscribe<TEvent>(Node owner, Action<TEvent> handler, int offset = 0);

        // Assina sem owner (você controla o Dispose)
        IDisposable Subscribe<TEvent>(Action<TEvent> handler, int offset = 0);
    }
}