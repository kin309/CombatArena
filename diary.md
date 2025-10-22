# Projeto:

## 2. Arena de Combate Automático (nível 1–2 – domínio e fluxo)

### Escopo:

- 2 personagens que atacam sozinhos

- cada ataque tem cooldown e dano

- vence quem ficar com mais HP

### Treina:

- Lógica de turnos e timers

- Sistema de eventos (ataque, morte, cura)

- Separação entre UI (barras, botões) e Domain (CombatManager, Fighter)

### Destaques:

- Excelente para testar logs e depuração visual

- Pode usar EffectQueue e EventBus como treino direto dos seus pontos fracos

- É pequeno, mas já exige integração real entre subsistemas

# [Dia 1]

Feito:
- Criei o repositório no Github
- Criei o projeto na Godot
- Criei a estrutura básica do projeto

Pretensão:
- Planejar o desenvolvimento

# [Dia 2]

Minha ideia de projeto mudou completamente, estou voltando a base, estudando os pilares para uma boa arquitetura de projetos.

Feito:
- Criei um núcleo de eventos, um eventbus + globalbus, eles servem para controlar todos os eventos que existem na nossa aplicação desde o que acontece
no combate, UI e logs, evitando acoplamentos desnecessários.

## Núcleo de eventos

⚙️ Principais recursos

Assinaturas tipadas (Subscribe<CoinCollected>)

Auto-unsubscribe → se o Node sai da cena, a inscrição é removida automaticamente.

Prioridade automática:

IGameplayEvent → 100

IUIEvent → 10

IAudioEvent → 0

ITelemetryEvent → -100

Ordenação garantida → os eventos são processados em ordem lógica (gameplay → UI → log).

🎮 Exemplo
GlobalBus.Subscribe<CoinCollected>(this, e => { /* atualiza inventário */ });
GlobalBus.Publish(new CoinCollected(1));

- Criei um sistema de logs completo.

⚙️ Principais recursos

Canais (LogChannel.Combat, UI, Net, etc.)

Níveis (Trace, Debug, Info, Warn, Error)

Tags e cores automáticas ([COMBAT] vermelho, [UI] azul)

Filtros globais e locais

Global → Log.Disable(LogChannel.Net)

Local → RichTextLog.Channels = LogChannel.Combat | LogChannel.UI

Auto-unsubscribe

Prioridade baixa (ITelemetryEvent) → logs sempre depois da lógica do frame

Múltiplos sinks simultâneos