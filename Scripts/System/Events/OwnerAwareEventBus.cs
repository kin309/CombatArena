// Systems/Events/OwnerAwareEventBus.cs
using System;
using System.Collections.Generic;
using Godot;

namespace Game.Systems.Events
{
    public sealed class OwnerAwareEventBus : IEventBus
    {
        private readonly IEventPriorityProvider _priorityProvider;

        public OwnerAwareEventBus(IEventPriorityProvider? provider = null)
        {
            _priorityProvider = provider ?? new DefaultEventPriorityProvider();
        }

        private sealed class Subscription : IDisposable
        {
            public Type EventType = default!;
            public Delegate Handler = default!;
            public int Priority;
            public bool Active = true;
            private readonly OwnerAwareEventBus _bus;
            private readonly Node? _owner; // null para manual
            public Subscription(OwnerAwareEventBus bus, Type evtType, Delegate handler, Node? owner, int priority)
            {
                _bus = bus; EventType = evtType; Handler = handler; _owner = owner;
            }
            public void Dispose()
            {
                if (!Active) return;
                Active = false;
                _bus.InternalUnsubscribe(this);
            }
            public Node? Owner => _owner;
        }

        // Handlers por tipo de evento
        private readonly Dictionary<Type, List<Subscription>> _byType = new();

        // Para auto-dispose: lista de subs por dono (Node)
        private readonly Dictionary<Node, List<Subscription>> _byOwner = new();

        // Para evitar múltiplos hooks no mesmo owner
        private readonly HashSet<Node> _hookedOwners = new();

        public void Publish<TEvent>(TEvent ev)
        {
            var type = typeof(TEvent);
            if (!_byType.TryGetValue(type, out var list)) return;

            // faz cópia para estabilidade durante iteração
            var snapshot = list.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                var sub = snapshot[i];
                if (!sub.Active) continue;
                if (sub.Handler is Action<TEvent> action)
                    action(ev);
            }
        }

        public IDisposable Subscribe<TEvent>(Node owner, Action<TEvent> handler, int offset = 0)
        {
            var basePrio = _priorityProvider.GetPriority(typeof(TEvent));
            var prio = basePrio + offset;

            var sub = new Subscription(this, typeof(TEvent), handler, owner, prio);
            AddToTypeListOrdered(sub);
            AddToOwnerList(owner, sub);
            EnsureOwnerHook(owner);
            return sub; // opcional: você pode chamar Dispose() manualmente se quiser
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> handler, int offset = 0)
        {
            var basePrio = _priorityProvider.GetPriority(typeof(TEvent));
            var prio = basePrio + offset;

            var sub = new Subscription(this, typeof(TEvent), handler, owner: null, prio);
            AddToTypeListOrdered(sub);
            return sub; // aqui é sua responsabilidade dar Dispose()
        }

        private void AddToTypeListOrdered(Subscription sub)
        {
            if (!_byType.TryGetValue(sub.EventType, out var list))
            {
                list = new List<Subscription>();
                _byType[sub.EventType] = list;
            }

            // Inserção ordenada por prioridade (maior primeiro)
            int i = list.FindIndex(s => sub.Priority > s.Priority);
            if (i >= 0) list.Insert(i, sub);
            else list.Add(sub);
        }

        private void AddToOwnerList(Node owner, Subscription sub)
        {
            if (!_byOwner.TryGetValue(owner, out var list))
            {
                list = new List<Subscription>();
                _byOwner[owner] = list;
            }
            list.Add(sub);
        }

        private void EnsureOwnerHook(Node owner)
        {
            if (_hookedOwners.Contains(owner)) return;

            // Quando o owner estiver saindo da árvore, limpamos todas as subs dele.
            owner.TreeExiting += () =>
            {
                if (_byOwner.TryGetValue(owner, out var list))
                {
                    // Cópia para não modificar enquanto iteramos
                    var copy = list.ToArray();
                    foreach (var sub in copy)
                        sub.Dispose(); // chama InternalUnsubscribe
                }
                _byOwner.Remove(owner);
                _hookedOwners.Remove(owner);
            };

            _hookedOwners.Add(owner);
        }

        private void InternalUnsubscribe(Subscription sub)
        {
            // remove do índice por tipo
            if (_byType.TryGetValue(sub.EventType, out var list))
                list.Remove(sub);

            // remove do índice por owner (se houver)
            var owner = sub.Owner;
            if (owner != null && _byOwner.TryGetValue(owner, out var olist))
                olist.Remove(sub);
        }
    }
}
